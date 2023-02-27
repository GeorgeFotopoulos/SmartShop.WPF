namespace SmartShop.Models;

public class Page : PropertyChangedBase
{
	private string _number;
	private bool _isCurrent;

	public Page(string number, bool isCurrent)
	{
		Number = number;
		IsCurrent = isCurrent;
	}

	public string Number { get => _number; set => SetField(ref _number, value); }
	public bool IsCurrent { get => _isCurrent; set => SetField(ref _isCurrent, value); }
}