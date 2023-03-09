using SmartShop.Utilities;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace SmartShop.Models;

public class Cart : PropertyChangedBase
{
	private ObservableCollection<Product> _items = new();

	public Cart()
	{
		Items.CollectionChanged += ItemsCollectionChanged;
	}

	public ObservableCollection<Product> Items { get => _items; set => SetField(ref _items, value); }

	public double TotalPrice => Items.Sum(x => x.FinalPrice);

	private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.Action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Remove)
		{
			NotifyPropertyChanged(nameof(TotalPrice));
		}
	}
}
