using System;
using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Transfer
{
    public class PrepareTransferException : Exception
    {
       
        public PrepareTransferException(string message) : base(message)
        {
        }

        public PrepareTransferException(TransactionCheckResult checkResult) : this(
            $"Failed to check transaction with result: {checkResult.ToString()}")
        {
        }
    }
}