namespace SmartShop.Models;

public class Product
{
	public string Code { get; set; }
	public string Store { get; set; }
	public string Link { get; set; }
	public string ProductName { get; set; }
	public double StartingPrice { get; set; }
	public double FinalPrice { get; set; }
	public double PricePerUnit { get; set; }
	public string MetricUnit { get; set; }
	public bool Discounted { get; set; }
	public double? DiscountPercentage => Discounted ? (StartingPrice - FinalPrice) / StartingPrice : null;
}
