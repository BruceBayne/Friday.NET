using System;
using System.IO;
using Newtonsoft.Json;

namespace Friday.Json.Basics
{
	public abstract class NewtonsoftJsonRepository : IEntityRepository
	{
		private const string FileExtension = ".json";
		protected JsonSerializerSettings JsonSettings;

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


		public void SaveEntity<T>(T entity)
		{
			SaveEntity(entity, GetFileNameForEntity(entity));
		}

		protected virtual string GetFileNameForEntity<T>(T entity)
		{
			return entity.GetType().Name;
		}

		private void SaveEntity(object entity, string fileName)
		{
			var serialized = JsonConvert.SerializeObject(entity, JsonSettings);
			var filePath = GetFullPath(fileName);
			File.WriteAllText(filePath, serialized);
		}

		private string GetFullPath(string fileName)
		{
			return Path.Combine(HomeDirectory, fileName + FileExtension);
		}



		public object LoadEntity(Type e)
		{

			var filePath = GetFullPath(GetFileNameForEntity(e));

			if (!File.Exists(filePath))
			{
				var defaultNewEntity = Activator.CreateInstance(e);
				SaveEntity(defaultNewEntity);
				return defaultNewEntity;
			}
			return JsonConvert.DeserializeObject(File.ReadAllText(filePath), e, JsonSettings);


		}

		public T LoadEntity<T>() where T : new()
		{
			return (T)LoadEntity(typeof(T));
		}
	}
}