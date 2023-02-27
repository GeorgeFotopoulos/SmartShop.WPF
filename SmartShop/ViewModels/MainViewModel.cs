using SmartShop.Commands;
using SmartShop.Models;
using SmartShop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SmartShop.ViewModels;

public class MainViewModel : PropertyChangedBase
{
	private const int ItemsPerPage = 30;
	private readonly List<Product> _data;
	private readonly IProductService _productService;

	private string _searchText;
	private ObservableCollection<Page> _pages = new();
	private int _totalItems, _totalPages, _currentPage;
	private ObservableCollection<Product> _products, _pagedProducts;

	public MainViewModel(IProductService productService)
	{
		_productService = productService;
		_data = _productService.GetProducts();

		// Initialize commands
		ClearCommand = new RelayCommand(ClearSearch);
		GoToPreviousPageCommand = new RelayCommand(() => CurrentPage--, () => CurrentPage > 1);
		GoToNextPageCommand = new RelayCommand(() => CurrentPage++, () => CurrentPage < TotalPages && TotalPages > 0);
		GoToPageCommand = new CommandHandler(page => GoToPage(page), true);

		Products = new ObservableCollection<Product>(_data);
	}

	public int TotalItems { get => _totalItems; private set => SetField(ref _totalItems, value); }
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
				TotalItems = _products.Count;
				TotalPages = (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
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
				Products = new ObservableCollection<Product>(_data.Where(p => p.ProductName.ToLower().Contains(SearchText.ToLower())));
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
				GoToPreviousPageCommand.RaiseCanExecuteChanged();
				GoToNextPageCommand.RaiseCanExecuteChanged();
			}

			UpdatePagedItems();
			LoadPageNumbers();
			SetIsCurrent();
		}
	}

	public RelayCommand SearchCommand { get; }
	public RelayCommand ClearCommand { get; }
	public RelayCommand GoToPreviousPageCommand { get; }
	public RelayCommand GoToNextPageCommand { get; }
	public ICommand GoToPageCommand { get; }

	private void ClearSearch()
	{
		SearchText = string.Empty;
		Products = new ObservableCollection<Product>(_data);
	}

	private void UpdatePagedItems()
	{
		var startIndex = (CurrentPage - 1) * ItemsPerPage;
		PagedProducts = new ObservableCollection<Product>(Products.Skip(startIndex).Take(ItemsPerPage));
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