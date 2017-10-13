using System.Linq;
using System.Threading.Tasks;
using Friday.ValueTypes.Currencies;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;

namespace Friday.Bitcoin.Services
{
    public class BitCoinBalanceLoader
    {
        private readonly int minimumConfirmations;

        private static readonly Network Network = Network.Main;


        public static Task<BalanceModel> GetAddressBalanceModel(BitcoinPubKeyAddress address)
        {
            var client = new QBitNinjaClient(Network);
            var t = client.GetBalance(address, true);
            return t;
        }



        public BitCoinBalanceLoader(int minimumConfirmations = 3)
        {
            this.minimumConfirmations = minimumConfirmations;
        }

        public Task<BitCoin> GetAddressBalance(BitcoinPubKeyAddress address)
        {
            var tcs = new TaskCompletionSource<BitCoin>();


            var addressModelTask = GetAddressBalanceModel(address);

            addressModelTask.ContinueWith((task, bm) =>
            {
                var satoshi = task.Result.Operations.Where(x => x.Confirmations >= minimumConfirmations)
                    .Sum(x => x.Amount.Satoshi);
                var balance = BitCoin.FromSatoshi((ulong)satoshi);
                tcs.SetResult(balance);
            }, TaskContinuationOptions.NotOnFaulted);


            addressModelTask.ContinueWith((task, bm) =>
            {
                if (task.IsFaulted && task.Exception != null)
                    tcs.TrySetException(task.Exception);
            }, TaskContinuationOptions.OnlyOnFaulted);

            return tcs.Task;
        }
    }
}
