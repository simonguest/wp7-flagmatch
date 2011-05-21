using System;
using System.Windows.Input;

namespace FlagMatch.Commands
{
    public class HighScoreContinueCommand : ICommand
    {
        public HighScoreContinueCommand()
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
                if (CommandEvents.HighScoreContinueButtonPressed != null)
                {
                    CommandEvents.HighScoreContinueButtonPressed(this, new EventArgs());
                }
            }
        }
    }
}
