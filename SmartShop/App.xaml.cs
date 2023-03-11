using Autofac;
using Autofac.Core;
using SmartShop.DataAccess;
using SmartShop.Models;
using SmartShop.Services;
using SmartShop.ViewModels;
using SmartShop.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartShop;

public partial class App : Application
{
	private IContainer _container;

	private void OnStartup(object sender, StartupEventArgs e)
	{
		// Creates the container builder
		var builder = new ContainerBuilder();

		// Registers Services & Dependencies
		builder.RegisterType<DatabaseService>().As<IDatabaseService>()
			.WithParameter("databasePath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "database.db"));

		builder.RegisterType<CartService>().As<ICartService>().SingleInstance();
		builder.RegisterType<ProductService>().As<IProductService>()
			.WithParameter(new ResolvedParameter(
				(pi, ctx) => pi.ParameterType == typeof(IDatabaseService),
				(pi, ctx) => ctx.Resolve<IDatabaseService>()
			));

		// Registers ViewModels
		builder.RegisterType<MainViewModel>().AsSelf()
			.WithParameter(new NamedParameter("discountMode", false))
			.WithParameter(new ResolvedParameter(
				(pi, ctx) => pi.ParameterType == typeof(IProductService),
				(pi, ctx) => ctx.Resolve<IProductService>()))
			.WithParameter(new ResolvedParameter(
				(pi, ctx) => pi.ParameterType == typeof(ICartService),
				(pi, ctx) => ctx.Resolve<ICartService>()))
			.WithParameter(new ResolvedParameter(
				(pi, ctx) => pi.ParameterType == typeof(IComponentContext),
				(pi, ctx) => ctx.Resolve<IComponentContext>()))
			.SingleInstance();

		builder.RegisterType<CartViewModel>().AsSelf()
			.WithParameter(new ResolvedParameter(
				(pi, ctx) => pi.ParameterType == typeof(IProductService),
				(pi, ctx) => ctx.Resolve<IProductService>()))
			.WithParameter(new ResolvedParameter(
				(pi, ctx) => pi.ParameterType == typeof(ICartService),
				(pi, ctx) => ctx.Resolve<ICartService>()))
			.WithParameter(
			 new ResolvedParameter(
				(pi, ctx) => pi.Name == "products",
				(pi, ctx) => ctx.Resolve<List<Product>>()))
			.InstancePerDependency();

		builder.RegisterType<ProductHistoryViewModel>().AsSelf()
			.WithParameter(
			 new ResolvedParameter(
				(pi, ctx) => pi.Name == "products",
				(pi, ctx) => ctx.Resolve<List<ProductHistory>>()))
			.InstancePerDependency();

		// Registers Views
		builder.RegisterType<MainWindow>().AsSelf();
		builder.RegisterType<CartWindow>().AsSelf();
		builder.RegisterType<ProductHistoryWindow>().AsSelf();

		// Builds the container
		_container = builder.Build();

		// Resolves the MainViewModel instance from the container
		var mainViewModel = _container.Resolve<MainViewModel>();

		// Resolves the MainWindow instance from the container
		var mainWindow = _container.Resolve<MainWindow>();
		mainWindow.DataContext = mainViewModel;
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
