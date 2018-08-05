using System;
using System.IO;
using Friday.Base.Storage;
using Newtonsoft.Json;

namespace Friday.Json.Basics
{
	public abstract class NewtonsoftJsonRepository : IEntityRepository
	{
		private const string FileExtension = ".json";
		protected readonly JsonSerializerSettings JsonSettings;

		protected abstract string GetHomePath();


		private string HomeDirectory
		{
			get
			{
				var savePath = GetHomePath();
				if (!Directory.Exists(savePath))
					Directory.CreateDirectory(savePath);
				return savePath;
			}
		}


		protected NewtonsoftJsonRepository()
		{
			JsonSettings = JsonRepositorySettings.Default;
		}


		public void RemoveEntity(string entityName)
		{
			var fileToRemove = GetFullPath(entityName);
			if (File.Exists(fileToRemove))
				File.Delete(fileToRemove);
		}



		public void SaveEntity<T>(string entityName, T entity) where T : new()
		{
			var serialized = JsonConvert.SerializeObject(entity, JsonSettings);
			var filePath = GetFullPath(entityName);
			File.WriteAllText(filePath, serialized);

		}



		private string GetFullPath(string fileName)
		{
			return Path.Combine(HomeDirectory, fileName + FileExtension);
		}

		public object LoadOrDefaultEntity(string entityName, Type e)
		{

			var filePath = GetFullPath(entityName);

			if (!File.Exists(filePath))
			{
				var defaultNewEntity = Activator.CreateInstance(e);
				SaveEntity(entityName, defaultNewEntity);
				return defaultNewEntity;
			}
			return JsonConvert.DeserializeObject(File.ReadAllText(filePath), e, JsonSettings);


		}

		public T LoadOrDefaultEntity<T>(string entityName) where T : new()
		{
			return (T)LoadOrDefaultEntity(entityName, typeof(T));
		}
	}
}