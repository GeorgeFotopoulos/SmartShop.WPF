using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartShop.Converters;

public class CountToBooleanConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value switch
		{
			int count => count > 0,
			double count => count > 0,
			long count => count > 0,
			_ => (object)false,
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
