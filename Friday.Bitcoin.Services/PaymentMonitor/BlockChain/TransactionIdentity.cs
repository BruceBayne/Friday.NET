using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.BlockChain
{

    public struct TransactionIdentity
    {
        public int BlockHeight { get; set; }
        public uint256 TransactionId;

        public override string ToString()
        {
            return $"{nameof(BlockHeight)}: {BlockHeight}, {nameof(TransactionId)}: {TransactionId}";
        }
    }


}