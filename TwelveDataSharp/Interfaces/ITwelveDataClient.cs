using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwelveDataSharp.Library.ResponseModels;

namespace TwelveDataSharp.Interfaces
{
    public interface ITwelveDataClient
    {
        Task<TwelveDataQuote> GetQuoteAsync(string symbol, string interval = "1min");
        Task<TwelveDataPrice> GetRealTimePriceAsync(string symbol);
        Task<TwelveDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval = "1min");
        Task<TwelveDataTimeSeriesAverage> GetTimeSeriesAverageAsync(string symbol, string interval = "1min");
        Task<TwelveDataAdx> GetAdxValuesAsync(string symbol, string interval = "1min");
        Task<TwelveDataBollingerBands> GetBollingerBandsAsync(string symbol, string interval = "1min");
    }
}
