using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwelveDataSharp.Models;
using TwelveDataSharp.DataModels;

/*
 * TwelveDataSharp - a .NET Standard 2.0 library to access stock market data from Twelve Data
 * Author - Shravan Jambukesan <shravan@shravanj.com>
 * https://github.com/pseudomarkets/TwelveDataSharp
 */

namespace TwelveDataSharp
{
    public class TwelveDataClient
    {
        private string apiKey = "";

        public TwelveDataClient(string key)
        {
            apiKey = key;
        }

        /*
         * Asynchronously get a quote for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
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

        /*
         * Asynchronously get a real time price for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         */
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

        /*
         * Asynchronously get time series data for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
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

        /*
         * Asynchronously get time series averages for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
        public async Task<TwelveDataTimeSeriesAverage> GetTimeSeriesAverageAsync(string symbol, string interval = "1min")
        {
            var client = new HttpClient();
            string endpoint = "https://api.twelvedata.com/time_series?avg=" + symbol + "&interval=" + interval + "&apikey=" + apiKey;
            var response = await client.GetAsync(endpoint);
            string responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TechnicalIndicatorAvg>(responseString);
            List<TimeSeriesAverages> values = new List<TimeSeriesAverages>();
            var tsAverageValues = jsonResponse.Values;
            foreach (TIValue v in tsAverageValues)
            {
                TimeSeriesAverages val = new TimeSeriesAverages()
                {
                    AverageValue = Convert.ToDouble(v?.Avg),
                    Datetime = v.Datetime,
                };
                values.Add(val);
            }

            TwelveDataTimeSeriesAverage timeSeriesAverage = new TwelveDataTimeSeriesAverage()
            {
                Symbol = jsonResponse?.Meta?.Symbol,
                Interval = jsonResponse?.Meta?.Interval,
                Currency = jsonResponse?.Meta?.Interval,
                ExchangeTimezone = jsonResponse?.Meta?.ExchangeTimezone,
                Exchange = jsonResponse?.Meta?.Exchange,
                Type = jsonResponse?.Meta?.Type,
                IndicatorName = jsonResponse?.Meta?.Indicator?.Name,
                SeriesType = jsonResponse?.Meta?.Indicator?.SeriesType,
                TimePeriod = jsonResponse.Meta.Indicator.TimePeriod,
                Values = values
            };

            return timeSeriesAverage;
        }

    }
}
