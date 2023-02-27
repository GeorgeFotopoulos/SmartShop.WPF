using Microsoft.Extensions.DependencyInjection;
using SmartShop.DataAccess;
using SmartShop.Services;
using SmartShop.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartShop;

public partial class App : Application
{
	private readonly ServiceProvider _serviceProvider;

	public App()
	{
		var services = new ServiceCollection();
		ConfigureServices(services);
		_serviceProvider = services.BuildServiceProvider();
	}

	private static void ConfigureServices(ServiceCollection services)
	{
		var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "database.db");
		services.AddSingleton<IDatabaseService>(new SqliteService(databasePath));
		services.AddScoped<IProductService, ProductService>();
		services.AddSingleton<MainWindow>();
	}

	private void OnStartup(object sender, StartupEventArgs e)
	{
		var mainWindow = _serviceProvider.GetService<MainWindow>();
		mainWindow.Show();
	}

	private void DataGridCell_MouseEnterEvent(object sender, MouseEventArgs e)
	{
		if (sender is DataGridCell { Content: TextBlock str } && str.Text.Length > 40)
		{
			((DataGridCell)sender).ToolTip = str.Text;
		}
	}

	private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var dataGrid = sender as DataGrid;
		if (dataGrid?.SelectedItem != null)
		{
			dataGrid.ScrollIntoView(dataGrid.SelectedItem);
		}
	}
}
