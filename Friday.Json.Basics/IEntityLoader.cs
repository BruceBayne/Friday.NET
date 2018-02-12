using System;

namespace Friday.Json.Basics
{
	public interface IEntityLoader
	{
		object LoadEntity(Type e);
		T LoadEntity<T>() where T : new();
	}
}