using System;
using System.Threading.Tasks;

namespace Friday.Base.Storage
{
	public interface IEntityLoader
	{
		object LoadEntity(string entityName, Type e);
		T LoadEntity<T>(string entityName) where T : new();
	}

	public interface IEntityLoaderAsync
	{
		Task<object> LoadEntity(string entityName, Type e);
		Task<T> LoadEntity<T>(string entityName) where T : new();
	}
}