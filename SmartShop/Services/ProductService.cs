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
					Store = reader.GetString(1),
					Link = reader.GetString(2),
					ProductName = reader.GetString(3),
					StartingPrice = reader.GetDouble(4),
					FinalPrice = reader.GetDouble(5),
					PricePerUnit = !reader.IsDBNull(6) ? reader.GetDouble(6) : null,
					MetricUnit = !reader.IsDBNull(7) ? reader.GetString(7) : null,
					Discounted = reader.GetBoolean(8),
				};

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
