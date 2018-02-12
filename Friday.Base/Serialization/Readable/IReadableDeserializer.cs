using System;

namespace Friday.Base.Serialization.Readable
{
	public interface IReadableDeserializer
	{
		object Deserialize(string text, Type type);
		T Deserialize<T>(string text);
	}
}