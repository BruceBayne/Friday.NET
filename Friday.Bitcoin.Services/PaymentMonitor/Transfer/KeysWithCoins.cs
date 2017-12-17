using System.Collections.Generic;
using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Transfer
{
	public class KeysWithCoins : Dictionary<BitcoinSecret, List<List<ICoin>>>
	{

		public void Append(BitcoinSecret secret, List<ICoin> transaction)
		{

			if (ContainsKey(secret))
			{
				this[secret].Add(transaction);
			}
			else
			{
				Add(secret, new List<List<ICoin>>() { transaction });
			}

		}
	}
}