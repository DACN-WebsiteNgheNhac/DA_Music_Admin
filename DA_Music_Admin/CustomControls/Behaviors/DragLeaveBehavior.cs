using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace CustomControls.Behaviors
{
    public class DragLeaveBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(DragLeaveBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DragLeave += AssociatedObject_DragLeave;
        }

        private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            if (Command?.CanExecute(e) == true)
            {
                Command.Execute(e);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
        }

    }
}
