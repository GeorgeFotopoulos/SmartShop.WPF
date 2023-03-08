using SmartShop.Models;
using SmartShop.Services;
using SmartShop.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace SmartShop.Views;

public partial class ShoppingCartWindow : Window
{
	public ShoppingCartWindow(IProductService productService, ObservableCollection<Product> cartItems)
	{
		InitializeComponent();
		DataContext = new ShoppingCartViewModel(productService, cartItems);
	}
}
