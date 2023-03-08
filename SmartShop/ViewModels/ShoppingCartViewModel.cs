using SmartShop.Models;
using SmartShop.Services;
using SmartShop.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartShop.ViewModels;

public class ShoppingCartViewModel : PropertyChangedBase
{
	private readonly List<Product> _products;
	private readonly IProductService _productService;
	private readonly List<Correlation> _correlations;
	private ObservableCollection<Product> _sklavenitisProducts, _abProducts;

	public ShoppingCartViewModel(IProductService productService, ObservableCollection<Product> cartItems)
	{
		_productService = productService;

		_products = _productService.GetProducts();
		_correlations = _productService.GetCorrelations();

		var data = new List<Product>();
		foreach (var item in cartItems)
		{
			data.Add(item);
			data.Add(_products.FirstOrDefault(x => x.Code == _correlations.Where(x => x.Key == item.Code)
																 .Select(x => x.Value)
																 .FirstOrDefault()));
		}

		SklavenitisProducts = new(data.Where(x => x.Store.Equals("Σκλαβενίτης")));
		AbProducts = new (data.Where(x => x.Store.Equals("ΑΒ Βασιλόπουλος")));
	}

	public ObservableCollection<Product> SklavenitisProducts { get => _sklavenitisProducts; set => SetField(ref _sklavenitisProducts, value); }
	public ObservableCollection<Product> AbProducts { get => _abProducts; set => SetField(ref _abProducts, value); }
}
