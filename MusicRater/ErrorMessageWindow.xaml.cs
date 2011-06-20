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

namespace MusicRater
{
    public partial class ErrorMessageWindow : ChildWindow, IModalWindow
    {
        public ErrorMessageWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ErrorMessageWindow_Loaded);
        }

        void ErrorMessageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

