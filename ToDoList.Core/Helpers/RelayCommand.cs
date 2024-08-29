using System.Windows.Input;

namespace ToDoList.Core.Helpers
{
    public class RelayCommand : ICommand
    {
        private Action<object?> myAction;

        public event EventHandler? CanExecuteChanged;
        public RelayCommand(Action<object?> action)
        {
            myAction = action;
        }
        public RelayCommand(Action action)
        {
            myAction = (p) => action();
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            myAction(parameter);
        }
    }
}
