using Orleans;
using Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services.Grains
{
    public class StockGrain : Grain, IStockGrain
    {
        string price;

        public override async Task OnActivateAsync()
        {
            string stock;
            this.GetPrimaryKey(out stock);
            await UpdatePrice(stock);
            await base.OnActivateAsync();
        }

        async Task UpdatePrice(object stock)
        {
            price = await GetPriceFromYahoo(stock as string);
            Console.WriteLine(price);
        }

        public Task<string> GetPrice()
        {
            //System.Threading.Thread.Sleep(3000);
            Console.WriteLine(price);
            return Task.FromResult(price);
        }

        async Task<string> GetPriceFromYahoo(string stock)
        {
            var uri = "http://download.finance.yahoo.com/d/quotes.csv?f=snl1c1p2&e=.csv&s=" + stock;
            using (var http = new HttpClient())
            using (var resp = await http.GetAsync(uri))
            {
                return await resp.Content.ReadAsStringAsync();
            }
        }
    }
}
