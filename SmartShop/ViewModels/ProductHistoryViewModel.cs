using SmartShop.Models;
using SmartShop.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartShop.ViewModels;

public class ProductHistoryViewModel : PropertyChangedBase
{
	private ObservableCollection<ProductHistory> _historyItems;

	public ProductHistoryViewModel(List<ProductHistory> products)
	{
		HistoryItems = new ObservableCollection<ProductHistory>(products);
	}

	public ObservableCollection<ProductHistory> HistoryItems { get => _historyItems; set => SetField(ref _historyItems, value); }
}
