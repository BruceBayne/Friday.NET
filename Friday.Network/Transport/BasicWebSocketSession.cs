using System;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace Friday.Network.Transport
{
	public abstract class BasicWebSocketSession : WebSocketBehavior
	{
		public bool IsAlive => State == WebSocketState.Open;

		protected sealed override void OnMessage(MessageEventArgs e)
		{
			try
			{
				if (e.Type==Opcode.Text)
					ProcessTextMessage(e.Data);

				if (e.Type==Opcode.Binary)
					ProcessBinaryMessage(e.RawData);
			}

			catch (Exception ex)
			{
				ProcessException(ex);
			}
		}

		protected void CloseSession()
		{
			if (IsAlive)
				Context.WebSocket.Close(CloseStatusCode.Normal);
		}

		protected virtual void ProcessBinaryMessage(byte[] eRawData)
		{
		}


		protected abstract void ProcessException(Exception e);


		protected abstract void ProcessTextMessage(string eData);
	}
}