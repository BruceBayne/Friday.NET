using Friday.ValueTypes.Currencies;

namespace Friday.Bitcoin.Services.BlockChain.info.Exchange
{
    public static class BitcoinFormatter
    {

        public static string FormatUsingExchangeInfo(this BitCoin b, BitcoinExchangeRecord s)
        {
            var balance = (b.ToBtc()*(decimal)s.sell).ToString("0.000");
           
            return $"{b} / {s.CurrencyCode}: ~{balance}{s.symbol}";
        }
    }
}