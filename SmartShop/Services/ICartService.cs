using SmartShop.Models;

namespace SmartShop.Services;

public interface ICartService
{
	Cart GetCart();
	void AddToCart(Product product);
	void RemoveFromCart(Product product);
}