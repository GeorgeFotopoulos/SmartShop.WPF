using SmartShop.Models;

namespace SmartShop.Services;

public class CartService : ICartService
{
	private readonly Cart _shoppingCart = new Cart();

	public Cart GetCart()
	{
		return _shoppingCart;
	}

	public void AddToCart(Product product)
	{
		if (!_shoppingCart.Items.Contains(product))
		{
			_shoppingCart.Items.Add(product);
			product.IsInCart = true;
		}
	}

	public void RemoveFromCart(Product product)
	{
		if (_shoppingCart.Items.Contains(product))
		{
			_shoppingCart.Items.Remove(product);
			product.IsInCart = false;
		}
	}
}