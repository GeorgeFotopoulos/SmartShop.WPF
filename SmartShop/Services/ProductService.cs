using SmartShop.DataAccess;
using SmartShop.Models;
using System.Collections.Generic;
using System.Data;
using Xceed.Wpf.Toolkit.Primitives;

namespace SmartShop.Services;

public class ProductService : IProductService
{
	private readonly IDatabaseService _databaseService;

	public ProductService(IDatabaseService databaseService)
	{
		_databaseService = databaseService;
	}

	public List<Product> GetProducts()
	{
		var products = new List<Product>();

		var query = "SELECT * FROM products";
		using (var reader = _databaseService.Query<IDataReader>(query))
		{
			while (reader.Read())
			{
				products.Add(new Product
				{
					Id = reader.GetInt32(0),
					Shop = reader.GetString(1),
					Link = reader.GetString(2),
					ProductName = reader.GetString(3),
					Price = reader.GetString(4),
					PricePerUnit = reader.GetString(5)
				});
			}
		}

		return products;
	}

	//public void AddProduct(Product product)
	//{
	//	var query = "INSERT INTO products (Name, Age) VALUES (@Name, @Age)";
	//	databaseService.Execute(query, product.ProductName, product.Link);
	//}
}
