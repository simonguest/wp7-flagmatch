using System;
using System.Windows.Input;

namespace FlagMatch.Commands
{
    public class NewGameCommand : ICommand
    {
        public NewGameCommand()
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
                if (CommandEvents.NewGameButtonPressed != null)
                {
                    CommandEvents.NewGameButtonPressed(this, new EventArgs());
                }
            }
        }
    }

}
