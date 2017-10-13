using System.Threading.Tasks;
using NBitcoin;

namespace Friday.Bitcoin.Services.PaymentMonitor.Transfer
{
    public interface IPrepareTransfer
    {
        Task<Transaction> PrepareMoneyTransferAsync(TransferDetails ti);
    }
}