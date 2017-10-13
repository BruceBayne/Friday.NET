using Friday.ValueTypes.Currencies;
using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Transfer
{
    public struct TransferDetails
    {
        public BitCoin Amount;
        public BitCoin Fee;
        public BitcoinAddress Destination;
        public BitcoinAddress ChangeAddress;
    }
}