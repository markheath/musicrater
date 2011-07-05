using System.Windows;
using System.Windows.Controls;

namespace MusicRater.MvvmUtils
{
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as ChildWindow;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }
        public static void SetDialogResult(ChildWindow target, bool? value)
        {            
            target.SetValue(DialogResultProperty, value);
        }
        public static bool? GetDialogResult(ChildWindow target)
        {
            return (bool?)target.GetValue(DialogResultProperty);
        }

    }
}
