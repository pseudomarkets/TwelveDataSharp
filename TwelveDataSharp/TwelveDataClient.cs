using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwelveDataSharp.Models;
using TwelveDataSharp.DataModels;

namespace TwelveDataSharp
{
    public class TwelveDataClient
    {
        private string apiKey = "";

        public TwelveDataClient(string key)
        {
            apiKey = key;
        }

        public async Task<TwelveDataQuote> GetQuoteAsync(string symbol, string interval = "1min")
        {
            var client = new HttpClient();
            string endpoint = "https://api.twelvedata.com/quote?symbol=" + symbol + "&interval=" + interval + "&apikey=" + apiKey;
            var response = await client.GetAsync(endpoint);
            string responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TimeSeriesQuote>(responseString);
            TwelveDataQuote quote = new TwelveDataQuote()
            {
                Name = jsonResponse?.Name,
                Datetime = jsonResponse.Datetime,
                Open = Convert.ToDouble(jsonResponse?.Open),
                Close = Convert.ToDouble(jsonResponse?.Close),
                High = Convert.ToDouble(jsonResponse?.High),
                Low = Convert.ToDouble(jsonResponse?.Low),
                Volume = jsonResponse.Volume,
                PreviousClose = Convert.ToDouble(jsonResponse?.PreviousClose),
                Change = Convert.ToDouble(jsonResponse?.Change),
                PercentChange = Convert.ToDouble(jsonResponse?.PercentChange),
                AverageVolume = jsonResponse.AverageVolume,
                FiftyTwoWeekHigh = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.High),
                FiftyTwoWeekLow = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.Low),
                FiftyTwoWeekHighChange = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.HighChange),
                FiftyTwoWeekLowChange = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.LowChange),
                FiftyTwoWeekHighChangePercent = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.HighChangePercent),
                FiftyTwoWeekLowChangePercent = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.LowChangePercent),
                FiftyTwoWeekRange = jsonResponse?.FiftyTwoWeek?.Range
            };
            return quote;
        }

        public async Task<TwelveDataPrice> GetRealTimePriceAsync(string symbol)
        {
            var client = new HttpClient();
            string endpoint = "https://api.twelvedata.com/quote?symbol=" + symbol + "&apikey=" + apiKey;
            var response = await client.GetAsync(endpoint);
            string responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TimeSeriesRealTimePrice>(responseString);
            TwelveDataPrice realTimePrice = new TwelveDataPrice()
            {
                Price = Convert.ToDouble(jsonResponse?.Price)
            };
            return realTimePrice;
        }

        public async Task<TwelveDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval = "1min")
        {
            var client = new HttpClient();
            string endpoint = "https://api.twelvedata.com/time_series?symbol=" + symbol + "&interval=" + interval + "&apikey=" + apiKey;
            var response = await client.GetAsync(endpoint);
            string responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TimeSeriesStocks>(responseString);
            List<TimeSeriesValues> values = new List<TimeSeriesValues>();
            var tsStockValues = jsonResponse.Values;
            foreach(Value v in tsStockValues)
            {
                TimeSeriesValues val = new TimeSeriesValues()
                {
                    Datetime = v.Datetime,
                    Open = Convert.ToDouble(v?.Open),
                    High = Convert.ToDouble(v?.High),
                    Low = Convert.ToDouble(v?.Low),
                    Close = Convert.ToDouble(v?.Close),
                    Volume = v.Volume
                };
                values.Add(val);
            }
            TwelveDataTimeSeries timeSeries = new TwelveDataTimeSeries()
            {
                Symbol = jsonResponse?.Meta?.Symbol,
                Interval = jsonResponse?.Meta?.Interval,
                ExchangeTimezone = jsonResponse?.Meta?.ExchangeTimezone,
                Exchange = jsonResponse?.Meta?.Exchange,
                Type = jsonResponse?.Meta?.Type,
                Currency = jsonResponse?.Meta?.Currency,
                Values = values
            };
            return timeSeries;
        }

    }
}
