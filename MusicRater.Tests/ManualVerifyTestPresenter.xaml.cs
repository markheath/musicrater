using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;

namespace MusicRater.Tests
{
    public partial class ManualVerifyTestPresenter : UserControl
    {
        private WorkItemTest test;

        public ManualVerifyTestPresenter()
        {
            InitializeComponent();                
        }

        public void Init(string description, object content, WorkItemTest test)
        {
            this.textDescription.Text = description;
            this.testContent.Content = content;
            this.test = test;
        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            this.test.EnqueueTestComplete();
        }

        private void Fail_Click(object sender, RoutedEventArgs e)
        {            
            Assert.Fail();
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            Assert.Inconclusive();
        }

    }
}
