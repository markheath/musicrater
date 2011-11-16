using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using System;
using MusicRater.Model;

namespace MusicRater
{
    public class ConfigWindowViewModel : ViewModelBase
    {
        private readonly List<Criteria> originalCriteria;
        private readonly List<Criteria> editCriteria;
        private readonly ICommand updateCommand;
        private bool? dialogResult;

        public ConfigWindowViewModel(IEnumerable<Criteria> criteria)
        {
            this.originalCriteria = new List<Criteria>(criteria);
            this.editCriteria = (from c in originalCriteria select Clone(c)).ToList();
            this.updateCommand = new UpdateCriteriaCommand(originalCriteria, editCriteria, (dr) => this.DialogResult = dr);
        }

        private Criteria Clone(Criteria criteria)
        {
            return new Criteria(criteria.Name, criteria.Weight);
        }

        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }
            set
            {
                if (dialogResult != value)
                {
                    dialogResult = value;
                    RaisePropertyChanged("DialogResult");
                }
            }
        }
        public ICommand UpdateCommand { get { return this.updateCommand; } }

        public List<Criteria> Criteria { get { return editCriteria; }  }
    }

    class UpdateCriteriaCommand : ICommand
    {
        private readonly List<Criteria> original;
        private readonly List<Criteria> edited;
        private readonly Action<bool?> updateDialogResult;

        public UpdateCriteriaCommand(List<Criteria> original, List<Criteria> edited, Action<bool?> updateDialogResult)
        {
            this.original = original;
            this.edited = edited;
            this.updateDialogResult = updateDialogResult;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged = (o, e) => { };

        public void Execute(object parameter)
        {
            for (int n = 0; n < this.original.Count; n++)
            {
                this.original[n].Name = this.edited[n].Name;
                this.original[n].Weight = this.edited[n].Weight;
            }
            this.updateDialogResult(true);
        }
    }
}
