using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;
using System.Linq;

namespace SmartShop.DataAccess;

public class DatabaseService : IDatabaseService
{
	private readonly SqliteConnection connection;

	public DatabaseService(string folderId)
	{
		SQLitePCL.Batteries.Init();

		var service = new DriveService(new BaseClientService.Initializer
		{
			ApiKey = "AIzaSyDPUd_qY9XN6jV1NMlekPdYOkHfcpfFEGs",
			ApplicationName = "Your application name",
		});

		var fileList = service.Files.List();
		fileList.Q = $"'{folderId}' in parents and name='database.db'";
		var result = fileList.Execute();
		var file = result.Files.FirstOrDefault();

		if (file != null)
		{
			// Download the file to your application's directory
			var downloadRequest = service.Files.Get(file.Id);
			using var fileStream = new FileStream("database.db", FileMode.Create, FileAccess.Write);
			downloadRequest.Download(fileStream);
		}

		var connectionString = $"Data Source=database.db;Mode=ReadWriteCreate;";
		connection = new SqliteConnection(connectionString);
	}

	public void Execute(string query, params object[] parameters)
	{
		connection.Open();
		using var cmd = connection.CreateCommand();
		cmd.CommandText = query;
		foreach (var param in parameters)
		{
			cmd.Parameters.AddWithValue(null, param);
		}

		cmd.ExecuteNonQuery();
		connection.Close();
	}

	public IDataReader Query<T>(string query, params object[] parameters)
	{
		connection.Open();
		var cmd = connection.CreateCommand();
		cmd.CommandText = query;
		foreach (var param in parameters)
		{
			cmd.Parameters.AddWithValue(null, param);
		}

		var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
		return reader;
	}
}
