using SmartShop.Models;
using SmartShop.Services;
using SmartShop.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace SmartShop.Views;

public partial class MainWindow : Window
{
	private readonly MainViewModel _viewModel;

	public MainWindow(IProductService productService)
	{
		InitializeComponent();
		_viewModel = new MainViewModel(productService);
		DataContext = _viewModel;
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

	private void CartLink_Click(object sender, RoutedEventArgs e)
	{
		if (((Hyperlink)sender).Tag is Product product)
		{
			if (product.IsInCart)
			{
				_viewModel.CartItems.Remove(product);
				product.IsInCart = false;
			}
			else
			{
				_viewModel.CartItems.Add(product);
				product.IsInCart = true;
			}
		}
	}
}
