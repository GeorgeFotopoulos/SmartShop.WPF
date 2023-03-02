using SmartShop.DataAccess;
using SmartShop.Models;
using System.Collections.Generic;
using System.Data;

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
		Product product;

		var query = "SELECT * FROM products";
		using (var reader = _databaseService.Query<IDataReader>(query))
		{
			while (reader.Read())
			{
				product = new Product
				{
					Code = reader.GetString(0),
					Shop = reader.GetString(1),
					Link = reader.GetString(2),
					ProductName = reader.GetString(3),
					Price = reader.GetDouble(4),
					PricePerUnit = reader.GetDouble(5),
				};

				if (!reader.IsDBNull(6))
				{
					product.MetricUnit = reader.GetString(6);
				}

				products.Add(product);
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
