using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friday.ValueTypes.Currencies;
using NBitcoin;
using QBitNinja.Client;

namespace Friday.Bitcoin.Services.PaymentMonitor.Transfer
{
    public class PrepareTransfer : IPrepareTransfer
    {
        private const int MinConfirmations = 0;


        private readonly HashSet<BitcoinSecret> secrets = new HashSet<BitcoinSecret>();
        private readonly Network network = Network.Main;

        public PrepareTransfer()
        {
        }


        public async Task<Transaction> PrepareMoneyTransferAsync(TransferDetails ti)
        {
            var txRecords = await LoadInputTransactionsUntilAmount(ti.Amount + ti.Fee);

            var txBuilder = new TransactionBuilder();
            var tx = txBuilder
                .Send(ti.Destination, Money.Satoshis(ti.Amount.SatoshiAmount))
                .SendFees(Money.Satoshis(ti.Fee.SatoshiAmount))
                .SetChange(ti.ChangeAddress);


            foreach (var record in txRecords)
            {
                tx.AddKeys(record.Key);
                foreach (var transaction in record.Value)
                {
                    tx.AddCoins(transaction);
                }
            }

            var newTransaction = tx.BuildTransaction(true);
            var errors = txBuilder.Check(newTransaction);
            if (errors.Any())
            {
                var msg = string.Join(Environment.NewLine, errors.Select(x => x.ToString()));
                throw new PrepareTransferException(msg);
            }
            if (!txBuilder.Verify(newTransaction))
            {
                throw new PrepareTransferException("Can't verify transaction");
            }

            txBuilder.SignTransaction(newTransaction);
            var checkResult = newTransaction.Check();
            if (checkResult != TransactionCheckResult.Success)
            {
                throw new PrepareTransferException(checkResult);
            }

            return newTransaction;
        }


        private async Task<KeysWithTransactions> LoadInputTransactionsUntilAmount(BitCoin amountToSpent)
        {
            var result = new KeysWithTransactions();

            var acumulatedOnTransactions = BitCoin.Zero;

            foreach (var secret in secrets)
            {
                var client = new QBitNinjaClient(network);
                var balanceModel =
                    await client.GetBalance(new BitcoinPubKeyAddress(secret.PubKey.ToString(Network.Main)), true);


                foreach (var operation in balanceModel.Operations.Where(t =>
                    t.Amount > 0 && t.Confirmations >= MinConfirmations))
                {
                    var transactionResponse = await client.GetTransaction(operation.TransactionId);
                    result.Apend(secret, transactionResponse.Transaction);

                    acumulatedOnTransactions = acumulatedOnTransactions + BitCoin.FromSatoshi(operation.Amount);

                    if (acumulatedOnTransactions >= amountToSpent)
                    {
                        return result;
                    }
                }
            }

            throw new PrepareTransferException("Not enough coins");
        }


        public void AddSecretKey(BitcoinSecret secret)
        {
            secrets.Add(secret);
        }

        public void ResetSecrets()
        {
            secrets.Clear();
        }
    }
}