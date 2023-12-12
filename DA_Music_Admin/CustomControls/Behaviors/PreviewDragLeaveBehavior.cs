using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace CustomControls.Behaviors
{
    public class PreviewDragLeaveBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(PreviewDragLeaveBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewDragLeave += AssociatedObject_DragLeave;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewDragLeave -= AssociatedObject_DragLeave;
        }

        private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            if (Command?.CanExecute(e) == true)
            {
                Command.Execute(e);
            }
        }

       

    }
}
