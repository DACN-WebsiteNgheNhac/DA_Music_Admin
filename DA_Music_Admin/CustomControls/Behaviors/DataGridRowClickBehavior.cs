using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace CustomControls.Behaviors
{
    public class DataGridRowClickBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(DataGridRowClickBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
        }
        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Command?.CanExecute(e) == true)
            {
                Command.Execute(e);
            }
        }

    }
}
