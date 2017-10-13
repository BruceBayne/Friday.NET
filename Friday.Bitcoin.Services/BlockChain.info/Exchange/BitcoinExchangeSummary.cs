using System;

namespace Friday.Bitcoin.Services.BlockChain.info.Exchange
{
    public struct BitcoinExchangeSummary
    {
        public BitcoinExchangeRecord Usd;



        public override string ToString()
        {
            return $"{nameof(Usd)}: {Usd}, {nameof(Eur)}: {Eur}, {nameof(Rub)}: {Rub}, {nameof(UpdatedAt)}: {UpdatedAt}";
        }

        public BitcoinExchangeRecord Eur;
        public BitcoinExchangeRecord Rub;
        public DateTime UpdatedAt;
    }
}
