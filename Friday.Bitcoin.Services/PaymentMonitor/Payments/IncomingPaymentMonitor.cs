using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Friday.Bitcoin.Services.PaymentMonitor.BlockChain;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;

namespace Friday.Bitcoin.Services.PaymentMonitor.Payments
{
	public sealed class IncomingPaymentMonitor : IIncomingPaymentMonitor
	{
		private Task task;
		public event EventHandler<MoneyEventInfo> OnAmountChanged;
		public event EventHandler OnNewCheckCycleStarted;

		public event EventHandler<Exception> OnException;

		private readonly ConcurrentBag<MonitoringBlock> monitoringList = new ConcurrentBag<MonitoringBlock>();
		private readonly PaymentMonitorSettings settings = new PaymentMonitorSettings();


		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		private CancellationToken CancellationToken => cancellationTokenSource.Token;

		public IncomingPaymentMonitor()
		{
		}

		public IncomingPaymentMonitor(PaymentMonitorSettings settings)
		{
			this.settings = settings;
		}


		public bool RegisterMonitoringAddress(MonitoringBlock lku)
		{
			if (monitoringList.Any(t => t.Address == lku.Address))
				return false;
			monitoringList.Add(lku);
			return true;
		}


		public void StopMonitor()
		{
			if (task == null)
				return;
			cancellationTokenSource.Cancel();
		}


		public void StartMonitor()
		{
			if (task != null)
				return;

			task = Task.Factory.StartNew(async () =>
			{
				do
				{
					try
					{
						await CheckChanges().ConfigureAwait(false);
						await Task.Delay(settings.RefreshInterval, CancellationToken).ConfigureAwait(false);
					}
					catch (Exception e)
					{
						DoOnException(e);
					}
				} while (!CancellationToken.IsCancellationRequested);
			}, CancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
		}


		private async Task CheckChanges()
		{
			var client = new QBitNinjaClient(settings.ApiUrl, Network.Main);

			OnNewCheckCycleStarted?.Invoke(this, EventArgs.Empty);

			foreach (var monitoringElement in monitoringList.ToList())
			{
				var from = new BlockFeature(SpecialFeature.Last);
				var until = new BlockFeature(monitoringElement.Until.BlockHeight);

				var balance =
					await client.GetBalanceBetween(new BalanceSelector(monitoringElement.Address), from, until);

				if (balance == null)
					throw new PaymentMonitorException($"Can't get {monitoringElement.Address} balance");


				var monitorUntilTxid = monitoringElement.Until.TransactionId;
				var oldToNewOperations = balance.Operations.Where(t => t.ReceivedCoins != null).ToList();
				oldToNewOperations.Reverse();

				foreach (var operation in oldToNewOperations)
				{
					var noNewOperations = operation.TransactionId == monitorUntilTxid;

					if (noNewOperations)
						break;


					if (operation.Confirmations < settings.MinConfirmations)
					{
						continue;
					}

					var lku = new TransactionIdentity
					{
						BlockHeight = operation.Height,
						TransactionId = operation.TransactionId,
					};


					var info = new MoneyEventInfo(monitoringElement.Address, lku,
						operation.ReceivedCoins.Cast<Coin>().Select(x => x.Amount).ToList());

					OnAmountChanged?.Invoke(this, info);
					UpdateElementIdentity(monitoringElement, lku);
				}
			}
		}

		private void UpdateElementIdentity(MonitoringBlock element, TransactionIdentity identity)
		{
			var u = monitoringList.FirstOrDefault(t => t.Address == element.Address);
			if (u != null)
			{
				u.Until = identity;
			}
		}


		public bool RegisterMonitoringAddress(BitcoinAddress bitcoinAddress)
		{
			if (monitoringList.Any(uid => uid.Address == bitcoinAddress))
				return false;
			var lku = new MonitoringBlock() { Address = bitcoinAddress };
			return RegisterMonitoringAddress(lku);
		}

		private void DoOnException(Exception e)
		{
			OnException?.Invoke(this, e);
		}
	}
}