using SmartShop.Models;
using System.Collections.ObjectModel;

namespace SmartShop.Services;

public class CartService : ICartService
{
	private readonly ObservableCollection<Product> _cartItems = new();

	public CartService()
	{
		CartItems = _cartItems;
	}

	public ObservableCollection<Product> CartItems { get; }

	public void AddToCart(Product product)
	{
		if (!CartItems.Contains(product))
		{
			CartItems.Add(product);
			product.IsInCart = true;
		}
	}

	public void RemoveFromCart(Product product)
	{
		if (CartItems.Contains(product))
		{
			CartItems.Remove(product);
			product.IsInCart = false;
		}
	}
}
