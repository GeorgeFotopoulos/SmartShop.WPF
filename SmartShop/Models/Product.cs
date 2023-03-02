namespace SmartShop.Models;

public class Product {
	public string Code { get; set; }
	public string Shop { get; set; }
	public string ProductName { get; set; }
	public string Link { get; set; }
	public double Price { get; set; }
	public double PricePerUnit { get; set; }
	public string MetricUnit { get; set; }
}
