using System.Collections.Generic;
using System.Linq;
using Friday.Bitcoin.Services.PaymentMonitor.BlockChain;
using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Payments
{
    public class MoneyEventInfo
    {
        public BitcoinAddress Address;

        public override string ToString()
        {
            long amount = Money.Sum(money => money.Satoshi);

            return $"{nameof(Address)}: {Address}, {nameof(TransactionIdentity)}: {TransactionIdentity} SatoshiAmount:{amount}";
        }

        public TransactionIdentity TransactionIdentity;
        public IList<Money> Money;
    }
}