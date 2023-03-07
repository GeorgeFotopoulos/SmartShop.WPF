using SmartShop.Models;
using System.Collections.Generic;

namespace SmartShop.Services;
public interface IProductService
{
	List<Product> GetProducts();
	List<Correlation> GetCorrelations();
}