using System.Threading.Tasks;

namespace Friday.Base.Storage
{
	public interface IEntitySaver
	{
		void SaveEntity<T>(string entityName, T entity) where T : new();
	}


	public interface IEntitySaverAsync
	{
		Task SaveEntityAsync<T>(string entityName, T entity) where T : new();
	}

}