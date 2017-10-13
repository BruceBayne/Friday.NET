using Friday.Bitcoin.Services.PaymentMonitor.BlockChain;
using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Payments
{
    public class MonitoringBlock
    {

        public MonitoringBlock()
        {
            Until.TransactionId = uint256.Zero;
            Until.BlockHeight = 0;
        }
        public BitcoinAddress Address;

        public override string ToString()
        {
            return $"{nameof(Address)}: {Address}, {nameof(Until)}: {Until}";
        }

        public TransactionIdentity Until;
    }
}