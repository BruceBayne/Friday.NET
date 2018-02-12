namespace Friday.Json.Basics
{
	public interface IEntitySaver
	{
		void SaveEntity<T>(T entity);
	}
}