using System.Data;

namespace SmartShop.DataAccess;

public interface IDatabaseService
{
	void Execute(string query, params object[] parameters);
	IDataReader Query<T>(string query, params object[] parameters);
}
