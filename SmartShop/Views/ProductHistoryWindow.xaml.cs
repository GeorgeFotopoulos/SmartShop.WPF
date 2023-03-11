using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace SmartShop.Views;

public partial class ProductHistoryWindow : Window
{
	public ProductHistoryWindow()
	{
		InitializeComponent();
	}

	private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = e.Uri.AbsoluteUri,
			UseShellExecute = true
		});

		e.Handled = true;
	}
}
