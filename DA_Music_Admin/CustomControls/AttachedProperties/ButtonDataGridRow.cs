using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace CustomControls.AttachedProperties
{
    public class ButtonDataGridRow
    {
        public static readonly DependencyProperty ButtonClickCommand =
            DependencyProperty.RegisterAttached(
                "ButtonClickCommand",
                typeof(ICommand),
                typeof(DataGridRowExtensions),
                new PropertyMetadata(null, ButtonClickCommandChanged));

        public static readonly DependencyProperty PreviewMouseLeftButtonDownCommandProperty =
            DependencyProperty.RegisterAttached(
                "PreviewMouseLeftButtonDownCommand",
                typeof(ICommand),
                typeof(DataGridRowExtensions),
                new PropertyMetadata(null, PreviewMouseLeftButtonDownCommandChanged));


        public static void SetPreviewMouseLeftButtonDownCommand(DataGridRow element, ICommand value)
        {
            element.SetValue(PreviewMouseLeftButtonDownCommandProperty, value);
        }

        public static ICommand GetPreviewMouseLeftButtonDownCommand(DataGridRow element)
        {
            return (ICommand)element.GetValue(PreviewMouseLeftButtonDownCommandProperty);
        }

        private static void PreviewMouseLeftButtonDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGridRow = (DataGridRow)d;
            if (e.NewValue is ICommand command)
            {
                dataGridRow.PreviewMouseLeftButtonDown += (sender, args) =>
                {
                    if (command.CanExecute(d))
                        command.Execute(d);
                };
            }
        }

        public static void SetButtonClickCommand(Button element, ICommand value)
        {
            element.SetValue(PreviewMouseLeftButtonDownCommandProperty, value);
        }

        public static ICommand GetButtonClickCommand(Button element)
        {
            return (ICommand)element.GetValue(PreviewMouseLeftButtonDownCommandProperty);
        }


        private static void ButtonClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btnExtension = (Button)d;
            if (e.NewValue is ICommand command)
            {
                btnExtension.Click += (sender, args) =>
                {
                    if (command.CanExecute(d))
                        command.Execute(d);
                };
            }
        }
    }


}
