using Microsoft.Win32;
using SmartShop.Commands;
using SmartShop.Models;
using SmartShop.Services;
using SmartShop.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartShop.ViewModels;

public class CartViewModel : PropertyChangedBase
{
	private readonly ICartService _cartService;
	private readonly IProductService _productService;

	private readonly List<Product> _products;
	private readonly List<Correlation> _correlations;
	private ObservableCollection<Product> _abProducts = new ObservableCollection<Product>();
	private ObservableCollection<Product> _sklavenitisProducts = new ObservableCollection<Product>();

	public CartViewModel(IProductService productService, ICartService cartService, List<Product> products)
	{
		_productService = productService;
		_cartService = cartService;
		_cartService.GetCart().PropertyChanged += CartPropertyChanged;
		_products = products;
		_correlations = _productService.GetCorrelations();

		var data = new List<Product>();
		foreach (var item in _cartService.GetCart().Items)
		{
			if (data.Contains(item))
			{
				continue;
			}

			data.Add(item);

			// In case of MainViewModel running with discountMode true, then this code provides error, because some of the codes from correlations lead to null
			var correlatedProduct = _products.FirstOrDefault(x => x.Code == _correlations.Where(x => x.Key == item.Code).Select(x => x.Value).FirstOrDefault());
			data.Add(correlatedProduct);
		}

		SklavenitisProducts = new ObservableCollection<Product>(data.Where(x => x.Store.Equals("Σκλαβενίτης")));
		AbProducts = new ObservableCollection<Product>(data.Where(x => x.Store.Equals("ΑΒ Βασιλόπουλος")));

		foreach (var product in SklavenitisProducts.Concat(AbProducts))
		{
			product.PropertyChanged += OnProductPropertyChanged;
		}

		ExportCartCommand = new RelayCommand(obj => ExportCart(), () => _cartService.GetCart().Items.Any());
	}

	public ObservableCollection<Product> SklavenitisProducts { get => _sklavenitisProducts; set => SetField(ref _sklavenitisProducts, value); }
	public ObservableCollection<Product> AbProducts { get => _abProducts; set => SetField(ref _abProducts, value); }
	public double TotalPrice => _cartService.GetCart().TotalPrice;

	public RelayCommand ExportCartCommand { get; }

	private void OnProductPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(Product.IsInCart))
		{
			var product = (Product)sender;
			if (product.IsInCart)
			{
				_cartService.AddToCart(product);
				ExportCartCommand.RaiseCanExecuteChanged();
			}
			else
			{
				_cartService.RemoveFromCart(product);
				ExportCartCommand.RaiseCanExecuteChanged();
			}
		}
	}

	private void CartPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(Cart.TotalPrice))
		{
			NotifyPropertyChanged(nameof(TotalPrice));
		}
	}

	public void ExportCart()
	{
		var productsInCart = _cartService.GetCart().Items;
		var fileContent = new StringBuilder();

		foreach (var product in productsInCart)
		{
			fileContent.AppendLine($"Κατάστημα: {product.Store}");
			fileContent.AppendLine($"Προϊόν: {product.ProductName}");
			fileContent.AppendLine($"Σύνδεσμος: {product.Link}");
			fileContent.AppendLine($"Τιμή: {product.FinalPrice:F2} €");
			fileContent.AppendLine($"Τιμή ανά μονάδα: {product.PricePerUnitWithMetricUnit}");
			if (product.DiscountPercentage.HasValue)
			{
				fileContent.AppendLine($"Ποσοστό έκπτωσης: {product.DiscountPercentage.Value.ToString("P2")}");
			}

			fileContent.AppendLine();
		}

		fileContent.AppendLine($"\nΣύνολο: {TotalPrice:F2} €");

		var saveFileDialog = new SaveFileDialog
		{
			Filter = "Text Files (*.txt)|*.txt",
			FileName = "CartExport.txt"
		};

		if (saveFileDialog.ShowDialog() == true)
		{
			File.WriteAllText(saveFileDialog.FileName, fileContent.ToString());
		}
	}
}
