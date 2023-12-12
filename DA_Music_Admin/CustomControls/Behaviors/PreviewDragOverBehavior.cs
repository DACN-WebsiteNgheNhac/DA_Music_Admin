using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace CustomControls.Behaviors
{
    public class PreviewDragOverBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(PreviewDragOverBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DragEnter -= AssociatedObject_PreviewDragOver;
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (Command?.CanExecute(e) == true)
            {
                Command.Execute(e);
            }
        }
    }
}
