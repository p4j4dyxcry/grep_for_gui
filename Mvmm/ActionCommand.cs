using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Grepper.Mvmm
{
    public class ActionCommand : ICommand
    {
        private readonly Action _action;
        public ActionCommand(Action command)
        {
            _action = command;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
        }

        public void OnCanExcuteChanged()
        {
            CanExecuteChanged?.Invoke(this,new EventArgs());
        }

        public event EventHandler CanExecuteChanged;
    }
}
