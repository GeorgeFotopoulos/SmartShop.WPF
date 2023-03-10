using Autofac;
using SmartShop.Commands;
using SmartShop.Models;
using SmartShop.Services;
using SmartShop.Utilities;
using SmartShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SmartShop.ViewModels;

public class MainViewModel : PropertyChangedBase
{
	private readonly List<Product> _data;
	private readonly ICartService _cartService;
	private readonly IProductService _productService;
	private readonly IComponentContext _componentContext;

	private string _searchText;
	private ObservableCollection<Product> _products, _pagedProducts;
	private int _totalPages, _currentPage, _itemsPerPage, _cartItems;
	private ObservableCollection<Page> _pages = new ObservableCollection<Page>();

	public MainViewModel(IProductService productService, ICartService cartService, IComponentContext componentContext, bool discountMode)
	{
		_productService = productService;
		_cartService = cartService;
		_componentContext = componentContext;

		_data = discountMode
			? _productService.GetProducts().Where(x => x.Discounted).OrderByDescending(x => x.DiscountPercentage).ToList()
			: _productService.GetProducts().OrderByDescending(x => x.DiscountPercentage).ThenBy(x => x.PricePerUnit).ToList();

		SetItemsPerPage();

		ClearCommand = new RelayCommand(obj => ClearSearch(), () => true);
		GoToPreviousPageCommand = new RelayCommand(obj => CurrentPage--, () => CurrentPage > 1);
		GoToNextPageCommand = new RelayCommand(obj => CurrentPage++, () => CurrentPage < TotalPages && TotalPages > 0);
		GoToPageCommand = new RelayCommand(page => GoToPage(page), () => true);
		ViewCartCommand = new RelayCommand(obj => ViewCart(), () => _cartService.GetCart().Items.Count > 0);
		CartLinkClickCommand = new RelayCommand(product => ChangeProductCartState(product), () => true);

		Products = new ObservableCollection<Product>(_data);
	}

	public int TotalPages { get => _totalPages; private set => SetField(ref _totalPages, value); }
	public ObservableCollection<Page> Pages { get => _pages; set => SetField(ref _pages, value); }
	public ObservableCollection<Product> PagedProducts { get => _pagedProducts; private set => SetField(ref _pagedProducts, value); }

	public ObservableCollection<Product> Products
	{
		get => _products;

		set
		{
			if (SetField(ref _products, value))
			{
				TotalPages = (int)Math.Ceiling((double)_products.Count / _itemsPerPage);
				CurrentPage = 1;
			}
		}
	}

	public string SearchText
	{
		get => _searchText;

		set
		{
			if (SetField(ref _searchText, value))
			{
				CurrentPage = 0;
				var searchTerms = AutoCorrect.Normalize(_searchText).ToUpper().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				Products = new ObservableCollection<Product>(_data.Where(p => searchTerms.All(s => AutoCorrect.Normalize(p.ProductName).ToUpper().Contains(s.Trim()))));
			}
		}
	}

	public int CurrentPage
	{
		get => _currentPage;

		set
		{
			if (SetField(ref _currentPage, value))
			{
				UpdatePagedItems();
				LoadPageNumbers();
				SetIsCurrent();
				GoToPreviousPageCommand.RaiseCanExecuteChanged();
				GoToNextPageCommand.RaiseCanExecuteChanged();
			}
		}
	}

	public int CartItems { get => _cartItems; set => SetField(ref _cartItems, value); }

	public RelayCommand ClearCommand { get; }
	public RelayCommand GoToPreviousPageCommand { get; }
	public RelayCommand GoToNextPageCommand { get; }
	public RelayCommand ViewCartCommand { get; }
	public RelayCommand CartLinkClickCommand { get; }
	public RelayCommand GoToPageCommand { get; }

	private void ChangeProductCartState(object parameter)
	{
		if (parameter is Product product)
		{
			if (product.IsInCart)
			{
				_cartService.RemoveFromCart(product);
				ViewCartCommand.RaiseCanExecuteChanged();
				CartItems = _cartService.GetCart().Items.Count;
			}
			else
			{
				_cartService.AddToCart(product);
				ViewCartCommand.RaiseCanExecuteChanged();
				CartItems = _cartService.GetCart().Items.Count;
			}
		}
	}

	private void ViewCart()
	{
		// Resolves the CartViewModel instance from the container
		var cartViewModel = _componentContext.Resolve<CartViewModel>(new NamedParameter("products", _data));

		// Resolves the CartWindow instance from the container
		var cartWindow = _componentContext.Resolve<CartWindow>();
		cartWindow.Closed += CartWindowClosed;
		cartWindow.DataContext = cartViewModel;
		cartWindow.Show();
	}

	private void CartWindowClosed(object sender, EventArgs e)
	{
		ViewCartCommand.RaiseCanExecuteChanged();
	}

	private void SetItemsPerPage()
	{
		_itemsPerPage = SystemParameters.WorkArea.Width >= 1920 ? 30 : 24;
	}

	private void ClearSearch()
	{
		SearchText = string.Empty;
		Products = new ObservableCollection<Product>(_data);
	}

	private void UpdatePagedItems()
	{
		var startIndex = (CurrentPage - 1) * _itemsPerPage;
		PagedProducts = new ObservableCollection<Product>(Products.Skip(startIndex).Take(_itemsPerPage));
	}

	public void GoToPage(object parameter)
	{
		var page = parameter as Page;
		if (int.TryParse(page.Number, out var pageNumber))
		{
			CurrentPage = pageNumber;
		}
	}

	private void SetIsCurrent()
	{
		var currentPage = Pages.FirstOrDefault(page => !page.Number.Equals("...") && int.Parse(page.Number) == CurrentPage);
		if (currentPage != null)
		{
			currentPage.IsCurrent = true;
		}
	}

	private void LoadPageNumbers()
	{
		Pages.Clear();

		if (TotalPages <= 10)
		{
			for (var i = 1; i <= TotalPages; i++)
			{
				Pages.Add(new Page(i.ToString(), false));
			}

			return;
		}

		Pages.Add(new Page("1", false));
		Pages.Add(new Page("2", false));
		Pages.Add(new Page("3", false));

		if (CurrentPage > 5)
		{
			if (CurrentPage <= TotalPages - 5)
			{
				Pages.Add(new Page("...", false));
				var start = CurrentPage - 1;
				var end = CurrentPage + 1;
				for (var i = start; i <= end; i++)
				{
					Pages.Add(new Page(i.ToString(), false));
				}

				Pages.Add(new Page("...", false));
			}
			else
			{
				var start = CurrentPage - 2;
				Pages.Add(new Page("...", false));
				for (var i = start; i <= TotalPages - 2; i++)
				{
					if (i == TotalPages - 2)
					{
						continue;
					}

					Pages.Add(new Page(i.ToString(), false));
				}
			}
		}
		else
		{
			switch (CurrentPage)
			{
				case 1:
					Pages.Add(new Page("...", false));
					break;
				case 2:
					Pages.Add(new Page(4.ToString(), false));
					Pages.Add(new Page("...", false));
					break;
				case 3:
					Pages.Add(new Page(4.ToString(), false));
					Pages.Add(new Page(5.ToString(), false));
					Pages.Add(new Page("...", false));
					break;
				case 4:
					Pages.Add(new Page(4.ToString(), true));
					Pages.Add(new Page(5.ToString(), false));
					Pages.Add(new Page(6.ToString(), false));
					Pages.Add(new Page("...", false));
					break;
				case 5:
					Pages.Add(new Page(4.ToString(), false));
					Pages.Add(new Page(5.ToString(), true));
					Pages.Add(new Page(6.ToString(), false));
					Pages.Add(new Page(7.ToString(), false));
					Pages.Add(new Page("...", false));
					break;
			}
		}

		Pages.Add(new Page((TotalPages - 2).ToString(), false));
		Pages.Add(new Page((TotalPages - 1).ToString(), false));
		Pages.Add(new Page(TotalPages.ToString(), false));
	}
}