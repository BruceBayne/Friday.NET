using System;
using System.Net.WebSockets;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace Friday.Network.Transport
{
	public abstract class BasicWebSocketSession : WebSocketBehavior
	{
		public bool IsAlive => State == WebSocketSharp.WebSocketState.Open;

		protected sealed override async void OnMessage(MessageEventArgs e)
		{
			try
			{
				if (e.IsText)
					await ProcessTextMessage(e.Data);

				if (e.IsBinary)
					await ProcessBinaryMessage(e.RawData);
			}

			catch (Exception ex)
			{
				await ProcessExceptionNvi(ex);
			}
		}

		protected void CloseSession()
		{
			if (IsAlive)
				Context.WebSocket.Close(CloseStatusCode.Normal);
		}

		protected virtual Task ProcessBinaryMessage(byte[] eRawData)
		{
			return Task.CompletedTask;
		}


		private async Task ProcessExceptionNvi(Exception e)
		{
			try
			{
				await ProcessException(e);
			}
			catch (Exception)
			{
				CloseSession();
			}
		}


		protected abstract Task ProcessException(Exception e);


		protected abstract Task ProcessTextMessage(string eData);
	}
}