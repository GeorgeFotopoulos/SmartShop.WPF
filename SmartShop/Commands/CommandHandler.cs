using System;
using System.Windows.Input;

namespace SmartShop.Commands;

public class CommandHandler : ICommand
{
	private readonly Action<object> _action;
	private readonly bool _canExecute;

	public CommandHandler(Action<object> action, bool canExecute)
	{
		_action = action;
		_canExecute = canExecute;
	}

	public event EventHandler CanExecuteChanged;

	public bool CanExecute(object parameter)
	{
		return _canExecute;
	}

	public void Execute(object parameter)
	{
		_action(parameter);
	}
}