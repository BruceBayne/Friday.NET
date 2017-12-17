using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friday.ValueTypes.Currencies;
using NBitcoin;
using NBitcoin.Policy;
using QBitNinja.Client;

namespace Friday.Bitcoin.Services.PaymentMonitor.Transfer
{
	public class PrepareTransfer : IPrepareTransfer
	{
		private const int MinConfirmations = 0;


		private readonly HashSet<BitcoinSecret> secrets = new HashSet<BitcoinSecret>();
		private readonly Network network = Network.Main;


		public void SetPolicy()
		{


		}


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


		private async Task<KeysWithCoins> LoadInputTransactionsUntilAmount(BitCoin amountToSpent)
		{
			var result = new KeysWithCoins();

			var accumulatedOnTransactions = BitCoin.Zero;

			foreach (var secret in secrets)
			{
				var client = new QBitNinjaClient(network);
				var balanceModel =
					await client.GetBalance(new BitcoinPubKeyAddress(secret.PubKey.ToString(Network.Main)), true);


				foreach (var operation in balanceModel.Operations.Where(t =>
					t.Amount > 0 && t.Confirmations >= MinConfirmations))
				{

					accumulatedOnTransactions = accumulatedOnTransactions + BitCoin.FromSatoshi(operation.Amount);
					result.Append(secret, operation.ReceivedCoins);

					if (accumulatedOnTransactions >= amountToSpent)
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