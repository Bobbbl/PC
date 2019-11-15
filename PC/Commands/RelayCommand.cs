using System;
using System.Windows.Input;

namespace PC
{
    public class RelayCommand : ICommand
    {
        private Action mAction;

        public delegate bool CanExecutePointer();
        public CanExecutePointer CANPointer { get; set; } = null;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged(null, null);
        }

        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public bool CanExecute(object parameter)
        {
            if (CANPointer == null)
            {
                return true;
            }

            return CANPointer();
        }

        public void Execute(object parameter)
        {
            mAction();
        }
    }
}
