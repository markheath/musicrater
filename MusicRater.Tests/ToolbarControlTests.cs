﻿using System;
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

        /*
        [TestMethod]
        public void ClickPlay()
        {
            Button b; //tc.Cont
            
            Assert.IsTrue(tc.ActualWidth > 0);
        }*/

        [TestMethod]
        [Asynchronous]
        public void WaitForABit()
        {
            // need to work out how to make it interactive here
            //this.EnqueueDelay(5000);
            viewModel.PlayCommand.Executed += (sender, args) => this.TestComplete();
        }        
    }

    [TestClass]
    public class ToolbarControlManualVerifyTests : SilverlightTest
    {
        private ManualVerifyTestPresenter presenter;
        private TestToolbarControlViewModel viewModel;

        [TestInitialize]
        public void SetUp()
        {
            this.presenter = new ManualVerifyTestPresenter();
            this.TestPanel.Children.Add(presenter);
        }

        [TestMethod]
        [Asynchronous]
        public void CheckToolTips()
        {
            this.presenter.Init("Confirm that each button has the correct tooltip.",
                new ToolbarControl(), this);
        }
        
        [TestMethod]
        [Asynchronous]
        public void CheckShuffledList()
        {
            var lb = new ListBox();
            lb.ItemsSource = new [] {1,2,3,4,5,6,7,8,9,10,11,12};
            this.presenter.Init("Confirm that these numbers are in random order.",
                lb, this);
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
