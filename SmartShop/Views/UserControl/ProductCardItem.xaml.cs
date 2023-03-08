using SmartShop.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SmartShop.Views.UserControl;

public partial class ProductCardItem
{
	public ProductCardItem()
	{
		InitializeComponent();
	}

	private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
	{
		// Get the link from the DataContext
		var link = ((sender as TextBlock)?.DataContext as Product)?.Link;
		if (link != null)
		{
			// Open the link in the default browser
			Process.Start(new ProcessStartInfo
			{
				FileName = link,
				UseShellExecute = true
			});
			e.Handled = true;
		}
	}
}