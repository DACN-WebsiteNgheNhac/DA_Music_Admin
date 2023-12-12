using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace CustomControls.Behaviors
{
    public class PreviewDropBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(PreviewDropBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewDrop -= AssociatedObject_PreviewDrop;
        }

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            if (Command?.CanExecute(e) == true)
            {
                Command.Execute(e);
            }
        }

       

    }
}
