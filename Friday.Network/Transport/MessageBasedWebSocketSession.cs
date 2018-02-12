using Friday.Base.Exceptions;
using Friday.Base.Network;
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


		public void Send(TServerMessage message)
		{
			var serialized = Serializer.Serialize(message);
			Send(serialized);
		}


		protected abstract void ProcessMessage(TClientMessage message);

		protected override void ProcessTextMessage(string message)
		{
			var objectFromPacket = GetObjectFromMessage(message);
			ProcessMessage(objectFromPacket);
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