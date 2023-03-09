using SmartShop.Models;

namespace SmartShop.Services;

public interface ICartService
{
	Cart ShoppingCart { get; }
	void AddToCart(Product product);
	void RemoveFromCart(Product product);
}