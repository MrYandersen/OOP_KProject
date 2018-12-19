using System;
using System.Windows.Input;

namespace KProject.Application.Utils
{
	public class RelayCommand : ICommand
	{
		private Action<object> _execute;
		private Predicate<object> _canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute ?? delegate { return true; };
		}

		public bool CanExecute(object param)
		{
			return _canExecute(param);
		}

		public void Execute(object param)
		{
			_execute(param);
		}
	}
}
