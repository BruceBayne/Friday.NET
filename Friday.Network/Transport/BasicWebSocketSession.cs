using System;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace Friday.Network.Transport
{
	public abstract class BasicWebSocketSession : WebSocketBehavior
	{
		public bool IsAlive => State == WebSocketState.Open;

		protected sealed override async void OnMessage(MessageEventArgs e)
		{
			try
			{
				if (e.IsText)
					await ProcessTextMessage(e.Data).ConfigureAwait(false);

				if (e.IsBinary)
					await ProcessBinaryMessage(e.RawData).ConfigureAwait(false);
			}

			catch (Exception ex)
			{
				await ProcessExceptionNvi(ex).ConfigureAwait(false);
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
				await ProcessException(e).ConfigureAwait(false);
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