using System;
using System.Windows.Input;

namespace FlagMatch.Commands
{
    public class Answer1Command : ICommand
    {
        public Answer1Command()
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
                if (CommandEvents.AnswerButtonPressed != null)
                {
                    CommandEvents.AnswerButtonPressed(this, new EventArgs());
                }
            }
        }
    }
}
