using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactBook.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
            _canExecute = predicate => true;
        }

        public RelayCommand(Action<object> execute, Predicate<object> predicate)
        {
            _execute = execute;
            _canExecute = predicate;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public virtual void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public void RaisCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
