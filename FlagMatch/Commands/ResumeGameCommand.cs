using System;
using System.Windows.Input;

namespace FlagMatch.Commands
{
    public class ResumeGameCommand : ICommand
    {
        public ResumeGameCommand()
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
                if (CommandEvents.ResumeGameButtonPressed != null)
                {
                    CommandEvents.ResumeGameButtonPressed(this, new EventArgs());
                }
            }
        }
    }

}
