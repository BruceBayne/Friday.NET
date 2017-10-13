using System.Collections.Generic;
using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Transfer
{
    public class KeysWithTransactions : Dictionary<BitcoinSecret,List<Transaction>>
    {
        public void Apend(BitcoinSecret secret, Transaction transaction)
        {

            if (ContainsKey(secret))
            {
                this[secret].Add(transaction);
            }
            else
            {
                Add(secret,new List<Transaction>(){transaction});
            }

        }
    }
}