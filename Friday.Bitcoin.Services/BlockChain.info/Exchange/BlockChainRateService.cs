using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Friday.Bitcoin.Services.BlockChain.info.Exchange
{
    public  class BlockChainRateService
    {

        public const string ApiUrl = "https://blockchain.info/ru/ticker";
        
        public  async Task<BitcoinExchangeSummary> GetExchangeRatesAsync()
        {
            
            var wc = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var s = await wc.DownloadStringTaskAsync(ApiUrl);
            var st = JsonConvert.DeserializeObject<BitcoinExchangeSummary>(s);
            
            st.Usd.CurrencyCode = nameof(st.Usd).ToUpper();
            st.Eur.CurrencyCode = nameof(st.Eur).ToUpper();
            st.Rub.CurrencyCode = nameof(st.Rub).ToUpper();

            st.UpdatedAt = DateTime.Now;
            return st;
        }


        
    }
}