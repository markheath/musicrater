using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Diagnostics;

namespace MusicRater.Model
{
    public class Rating : ViewModelBase
    {
        private int ratingValue;

        public Rating(Criteria criteria)
        {
            this.Criteria = criteria;
        }

        public Criteria Criteria { get; private set; }

        public int Value
        {
            get
            {
                return ratingValue;
            }
            set
            {
                if (this.ratingValue != value)
                {
                    this.ratingValue = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public string Name { get { return Criteria.Name; } }
    }
}
