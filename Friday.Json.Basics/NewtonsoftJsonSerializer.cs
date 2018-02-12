using System;
using System.Text;
using Friday.Base.Serialization;
using Friday.Base.Serialization.Readable;
using Newtonsoft.Json;

namespace Friday.Json.Basics
{
	public sealed class NewtonsoftJsonSerializer : ICompleteSerializer, ICompleteReadableSerializer
	{
		private static string SerializeToText(object packet)
		{
			return JsonConvert.SerializeObject(packet);
		}

		public byte[] Serialize(object packet)
		{
			return Encoding.UTF8.GetBytes(SerializeToText(packet));
		}

		public object Deserialize(byte[] buffer, Type type)
		{
			return Deserialize(GetStringFromBytes(buffer), type);
		}

		private static string GetStringFromBytes(byte[] buffer)
		{
			return Encoding.UTF8.GetString(buffer);
		}

		public T Deserialize<T>(byte[] buffer)
		{
			return Deserialize<T>(GetStringFromBytes(buffer));
		}

		string IReadableSerializer.Serialize(object packet)
		{
			return SerializeToText(packet);
		}

		public object Deserialize(string text, Type type)
		{
			return JsonConvert.DeserializeObject(text, type);
		}

		public T Deserialize<T>(string text)
		{
			return JsonConvert.DeserializeObject<T>(text);
		}
	}
}