using System.Collections.Generic;
using System.Linq;
using Friday.Bitcoin.Services.PaymentMonitor.BlockChain;
using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Payments
{
	public sealed class MoneyEventInfo
	{
		public readonly BitcoinAddress Address;
		public readonly long TotalSatoshi;
		public readonly TransactionIdentity TransactionIdentity;
		public readonly IList<Money> Money;

		public override string ToString()
		{
			return $"{nameof(Address)}: {Address}, {nameof(TransactionIdentity)}: {TransactionIdentity} TotalSatoshi:{TotalSatoshi}";
		}


		public MoneyEventInfo(BitcoinAddress address, TransactionIdentity transactionIdentity, IList<Money> money)
		{
			Address = address;
			TransactionIdentity = transactionIdentity;
			Money = money;

			if (Money != null)
				TotalSatoshi = Money.Sum(x => x.Satoshi);
		}
	}
}