using System.Runtime.Serialization;
using System.Threading.Tasks;
using Friday.Base.Exceptions;
using Friday.Base.Network;
using Friday.Base.Reflection;
using Friday.Base.Serialization.Readable;

namespace Friday.Network.Transport
{
	public abstract class MessageBasedWebSocketSession<TServerMessage, TClientMessage, TClientMessageType> : BasicWebSocketSession
		where TClientMessage : IMessageType<TClientMessageType>, new()
		where TServerMessage : class


	{
		private static readonly EnumToType<TClientMessageType, TClientMessage> EnumToType =
			new EnumToType<TClientMessageType, TClientMessage>();

		protected readonly ICompleteReadableSerializer Serializer;

		protected MessageBasedWebSocketSession(ICompleteReadableSerializer serializer)
		{
			Serializer = serializer;
		}


		public virtual void SendMessage(TServerMessage message)
		{
			if (message == null || !IsAlive)
				return;

			var serialized = Serializer.Serialize(message);
			Send(serialized);
		}


		protected abstract Task ProcessMessage(TClientMessage message);

		protected override async Task ProcessTextMessage(string message)
		{
			var objectFromPacket = GetObjectFromMessage(message);
			await ProcessMessage(objectFromPacket);
		}

		protected TClientMessage GetObjectFromMessage(string textMessage)
		{
			var basicMessageObject = Serializer.Deserialize<TClientMessage>(textMessage);

			if (!EnumToType.HasAppropriateTypeFor(basicMessageObject.MessageType))
				throw new TypeNotFoundException(basicMessageObject.MessageType.ToString());

			var type = EnumToType.GetType(basicMessageObject.MessageType);
			return (TClientMessage)Serializer.Deserialize(textMessage, type);
		}
	}
}