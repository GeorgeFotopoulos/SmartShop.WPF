using Microsoft.Data.Sqlite;
using System.Data;

namespace SmartShop.DataAccess;

public class DatabaseService : IDatabaseService
{
	private readonly SqliteConnection connection;

	public DatabaseService(string databasePath)
	{
		SQLitePCL.Batteries.Init();
		var connectionString = $"DataSource={databasePath};Mode=ReadWriteCreate;";
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
