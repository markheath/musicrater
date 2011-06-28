using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MusicRater.Tests
{
    [TestClass]
    public class ToolbarControlTests : SilverlightTest
    {
        private ToolbarControl tc;
        private TestToolbarControlViewModel viewModel;

        [TestInitialize]
        public void SetUp()
        {
            this.tc = new ToolbarControl();
            this.viewModel = new TestToolbarControlViewModel();
            this.tc.DataContext = viewModel;
            this.TestPanel.Children.Add(tc);
        }

        [TestMethod]
        public void DisplayDefaultSize()
        {
            Assert.IsTrue(tc.ActualWidth > 0);
        }

        [TestMethod]
        [Asynchronous]
        public void WaitForABit()
        {
            // need to work out how to make it interactive here
            //this.EnqueueDelay(5000);
            viewModel.PlayCommand.Executed += (sender, args) => this.TestComplete();
        }

        
    }
    public class TestToolbarControlViewModel
    {
        public TestableCommand PlayCommand { get; private set; }

        public TestToolbarControlViewModel()
        {
            this.PlayCommand = new TestableCommand();
        }

    }

    public class TestableCommand : ICommand
    {
        public event EventHandler Executed;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (this.Executed != null)
            { 
                this.Executed(this, EventArgs.Empty);
            }
        }
    }
}
