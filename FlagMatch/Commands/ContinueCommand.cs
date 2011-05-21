using System;
using System.Windows.Input;

namespace FlagMatch.Commands
{
    public class ContinueCommand : ICommand
    {
        public ContinueCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (this.CanExecute(null))
            {
                if (CommandEvents.ContinueButtonPressed != null)
                {
                    CommandEvents.ContinueButtonPressed(this, new EventArgs());
                }
            }
        }
    }
}
