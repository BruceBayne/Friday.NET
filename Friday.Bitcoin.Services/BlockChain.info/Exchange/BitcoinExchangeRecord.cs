namespace Friday.Bitcoin.Services.BlockChain.info.Exchange
{
    public struct BitcoinExchangeRecord
    {
        public double last;
        public double buy;
        public double sell;
        public string symbol;
        public string CurrencyCode;

        public string ToSimpleSellString()
        {
            return last + symbol;

        }


        public override string ToString()
        {
            return $"{nameof(last)}: {last}, {nameof(buy)}: {buy}, {nameof(sell)}: {sell}, {nameof(symbol)}: {symbol}";
        }
    }
}