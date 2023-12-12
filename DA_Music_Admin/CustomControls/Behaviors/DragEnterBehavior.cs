using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace CustomControls.Behaviors
{
    public class DragEnterBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(DragEnterBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (Command?.CanExecute(e) == true)
            {
                Command.Execute(e);
            }
        }


    }
}
