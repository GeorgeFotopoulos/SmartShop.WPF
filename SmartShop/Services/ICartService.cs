using SmartShop.Models;
using System.Collections.ObjectModel;

namespace SmartShop.Services;

public interface ICartService
{
	ObservableCollection<Product> CartItems { get; }
	void AddToCart(Product product);
	void RemoveFromCart(Product product);
}