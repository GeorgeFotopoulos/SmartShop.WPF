using System;
using System.Windows.Input;

namespace SmartShop.Commands;

public class RelayCommand : ICommand
{
	private readonly Action<object> _action;
	private readonly Func<object, bool> _canExecute;

	public RelayCommand(Action<object> action, Func<object, bool> canExecute = null)
	{
		_action = action ?? throw new ArgumentNullException(nameof(action));
		_canExecute = canExecute ?? (_ => true);
	}

	public event EventHandler CanExecuteChanged;

	public bool CanExecute(object parameter)
	{
		return _canExecute(parameter);
	}

	public void Execute(object parameter)
	{
		_action(parameter);
	}

	public void RaiseCanExecuteChanged()
	{
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}
