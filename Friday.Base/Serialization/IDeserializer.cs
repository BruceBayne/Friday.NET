using System;

namespace Friday.Base.Serialization
{
	public interface IDeserializer
	{
		object Deserialize(byte[] buffer, Type type);
		T Deserialize<T>(byte[] buffer);
	}
}