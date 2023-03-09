using SmartShop.Models;

namespace SmartShop.Services;

public class CartService : ICartService
{
	private readonly Cart _cart = new Cart();

	public Cart GetCart()
	{
		return _cart;
	}

	public void AddToCart(Product product)
	{
		if (!_cart.Items.Contains(product))
		{
			_cart.Items.Add(product);
			product.IsInCart = true;
		}
	}

	public void RemoveFromCart(Product product)
	{
		if (_cart.Items.Contains(product))
		{
			_cart.Items.Remove(product);
			product.IsInCart = false;
		}
	}
}