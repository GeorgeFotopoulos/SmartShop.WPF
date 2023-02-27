using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartShop.Utilities;

// https://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist
public class PropertyChangedBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value))
		{
			return false;
		}

		field = value;
		NotifyPropertyChanged(propertyName);
		return true;
	}
}