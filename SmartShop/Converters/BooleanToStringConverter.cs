using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartShop.Converters;

public class BooleanToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			var parameters = parameter?.ToString()?.Split('|');
			if (parameters != null && parameters.Length >= 2)
			{
				return boolValue ? parameters[0] : parameters[1];
			}
		}

		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
