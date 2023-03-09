using SmartShop.Models;

namespace SmartShop.Services;

public class CartService : ICartService
{
	private readonly Cart _shoppingCart = new();

	public CartService()
	{
		ShoppingCart = _shoppingCart;
	}

	public Cart ShoppingCart { get; set; }

	public void AddToCart(Product product)
	{
		if (!ShoppingCart.Items.Contains(product))
		{
			ShoppingCart.Items.Add(product);
			product.IsInCart = true;
		}
	}

	public void RemoveFromCart(Product product)
	{
		if (ShoppingCart.Items.Contains(product))
		{
			ShoppingCart.Items.Remove(product);
			product.IsInCart = false;
		}
	}
}
