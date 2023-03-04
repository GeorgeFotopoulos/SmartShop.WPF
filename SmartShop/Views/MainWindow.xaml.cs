using SmartShop.Services;
using SmartShop.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace SmartShop.Views;

public partial class MainWindow : Window
{
	private int _rowCount = 0;

	public MainWindow(IProductService productService)
	{
		InitializeComponent();
		DataContext = new MainViewModel(productService);
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

	private void DataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
	{
		e.Row.Header = (++_rowCount).ToString();
	}
}
