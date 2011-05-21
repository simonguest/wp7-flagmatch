using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlagMatch.Commands
{
    public static class CommandService
    {
        private static readonly DependencyProperty _commandProperty;

        static CommandService()
        {
            _commandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(CommandService),
               new PropertyMetadata(OnCommandChanged));
        }

        public static ICommand GetCommand(DependencyObject dependencyObject)
        {
            return (ICommand)dependencyObject.GetValue(_commandProperty);
        }

        public static void SetCommand(DependencyObject dependencyObject, ICommand value)
        {
            dependencyObject.SetValue(_commandProperty, value);
        }

        private static void OnCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dpceArgs)
        {
            if (dependencyObject is Button)
            {
                string parameter = dependencyObject.GetValue(_commandProperty).ToString();
                Button button = (Button)dependencyObject;
                ICommand command = (ICommand)dpceArgs.NewValue;
                button.Click += delegate(object sender, RoutedEventArgs arg) { command.Execute(parameter); };
            }
        }
    }

}
