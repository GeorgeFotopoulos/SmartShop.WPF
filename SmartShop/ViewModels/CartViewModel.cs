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
using System.Windows.Input;

namespace SmartShop.ViewModels;

public class CartViewModel : PropertyChangedBase
{
	private readonly ICartService _cartService;
	private readonly IProductService _productService;

	private readonly List<Product> _products;
	private readonly List<Correlation> _correlations;
	private ObservableCollection<Product> _sklavenitisProducts = new(), _abProducts = new();

	public CartViewModel(IProductService productService, ICartService cartService)
	{
		_productService = productService;
		_cartService = cartService;

		_products = _productService.GetProducts();
		_correlations = _productService.GetCorrelations();

		var data = new List<Product>();
		foreach (var item in _cartService.CartItems)
		{
			data.Add(item);
			data.Add(_products.FirstOrDefault(x => x.Code == _correlations.Where(x => x.Key == item.Code)
																 .Select(x => x.Value)
																 .FirstOrDefault()));
		}

		SklavenitisProducts = new(data.Where(x => x.Store.Equals("Σκλαβενίτης")));
		AbProducts = new(data.Where(x => x.Store.Equals("ΑΒ Βασιλόπουλος")));

		foreach (var product in _sklavenitisProducts.Concat(_abProducts))
		{
			product.PropertyChanged += OnProductPropertyChanged;
		}

		ExportCartCommand = new RelayCommand(ExportCart, () => SklavenitisProducts.Concat(AbProducts).Any(product => product.IsInCart));
	}

	public ObservableCollection<Product> SklavenitisProducts { get => _sklavenitisProducts; set => SetField(ref _sklavenitisProducts, value); }
	public ObservableCollection<Product> AbProducts { get => _abProducts; set => SetField(ref _abProducts, value); }
	public double TotalPrice => SklavenitisProducts.Concat(AbProducts).Where(product => product.IsInCart).Sum(product => product.FinalPrice);

	public ICommand ExportCartCommand { get; }

	private void OnProductPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		// Checks if the changed property is IsInCart, and if so, recalculates the TotalPrice property
		if (e.PropertyName == nameof(Product.IsInCart))
		{
			NotifyPropertyChanged(nameof(TotalPrice));
		}
	}

	public void ExportCart()
	{
		var productsInCart = SklavenitisProducts.Concat(AbProducts).Where(product => product.IsInCart);
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
