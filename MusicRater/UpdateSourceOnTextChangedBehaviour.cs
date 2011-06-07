using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace MusicRater
{
    /// <summary>
    /// this behaviour from http://jacob4u2.posterous.com/silverlight-4-textbox-update-source-on-text-c
    /// </summary>
    public class UpdateSourceOnTextChanged : Behavior<TextBox>
    {
        BindingExpression textBinding;

        protected override void OnAttached()
        {
            if (this.AssociatedObject == null)
                return;

            // Get the binding
            textBinding = this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);

            // Subscribe to text changed events.
            this.AssociatedObject.TextChanged += AssociatedObject_TextChanged;

            base.OnAttached();
        }

        void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update the binding source.
            if (textBinding != null)
                textBinding.UpdateSource();
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject == null)
                return;

            // Clean up...
            this.textBinding = null;
            this.AssociatedObject.TextChanged -= AssociatedObject_TextChanged;

            base.OnDetaching();
        }
    }


}
