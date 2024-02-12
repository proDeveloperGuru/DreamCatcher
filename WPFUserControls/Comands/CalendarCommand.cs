using System;
using System.Windows.Input;

namespace WPFUserControls.Comands
{
    public class CalendarCommand : ICommand
    {
        private readonly Action<object?> _action;
        private readonly Predicate<object?>? _canExecute;

        public CalendarCommand(Action<object?> action, Predicate<object?>? canExecute = null)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            _action = action;

            if(canExecute != null)
                _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameters)
        {
            return _canExecute == null ? true : _canExecute(parameters);
        }

        public void Execute(object? parameters)
        {
            _action(parameters);
        }
    }
}
