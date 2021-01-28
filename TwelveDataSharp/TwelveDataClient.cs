using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwelveDataSharp.Models;
using TwelveDataSharp.DataModels;
using TwelveDataSharp.Interfaces;

/*
 * TwelveDataSharp - a .NET Standard 2.0 library for accessing stock market data from Twelve Data
 * Author - Shravan Jambukesan <shravan@shravanj.com>
 * https://github.com/pseudomarkets/TwelveDataSharp
 */

namespace TwelveDataSharp
{
    public class TwelveDataClient : ITwelveDataClient
    {
        private string _apiKey = string.Empty;
        private readonly HttpClient _client;

        public TwelveDataClient(string key, HttpClient client)
        {
            _apiKey = key;
            _client = client;
        }

        /*
         * Asynchronously get a quote for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
        public async Task<TwelveDataQuote> GetQuoteAsync(string symbol, string interval = "1min")
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/quote?symbol=" + symbol + "&interval=" + interval + "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("TwelveDataSharp - GetRealTimePriceAsync could not retrieve data from Twelve Data, please check your API Key and interval");
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }

        }

        /*
         * Asynchronously get a real time price for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         */
        public async Task<TwelveDataPrice> GetRealTimePriceAsync(string symbol)
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/price?symbol=" + symbol + "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TimeSeriesRealTimePrice>(responseString);
                TwelveDataPrice realTimePrice = new TwelveDataPrice()
                {
                    Price = Convert.ToDouble(jsonResponse?.Price)
                };
                return realTimePrice;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("TwelveDataSharp - GetRealTimePriceAsync could not retrieve data from Twelve Data, please check your API Key and interval");
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }

        }

        /*
         * Asynchronously get time series data for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
        public async Task<TwelveDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval = "1min")
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/time_series?symbol=" + symbol + "&interval=" + interval + "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TimeSeriesStocks>(responseString);
                List<TimeSeriesValues> values = new List<TimeSeriesValues>();
                var tsStockValues = jsonResponse.Values;
                foreach (Value v in tsStockValues)
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("TwelveDataSharp - GetTimeSeriesAsync could not retrieve data from Twelve Data, please check your API Key and interval");
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }

        }

        /*
         * Asynchronously get time series averages for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
        public async Task<TwelveDataTimeSeriesAverage> GetTimeSeriesAverageAsync(string symbol, string interval = "1min")
        {
            try
            {
                var client = new HttpClient();
                string endpoint = "https://api.twelvedata.com/avg?symbol=" + symbol + "&interval=" + interval + "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TechnicalIndicatorAvg>(responseString);
                List<TimeSeriesAverages> values = new List<TimeSeriesAverages>();
                var tsAverageValues = jsonResponse.Values;
                foreach (AvgValue v in tsAverageValues)
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("TwelveDataSharp - GetTimeSeriesAverageAsync could not retrieve data from Twelve Data, please check your API Key and interval");
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }

        }

        /*
         * Asynchronously get average directional index values for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
        public async Task<TwelveDataAdx> GetAdxValuesAsync(string symbol, string interval = "1min")
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/adx?symbol=" + symbol + "&interval=" + interval + "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TechnicalIndicatorAdx>(responseString);
                List<AdxValues> values = new List<AdxValues>();
                var adxValues = jsonResponse.Values;
                foreach (AdxValue a in adxValues)
                {
                    AdxValues val = new AdxValues()
                    {
                        AdxValue = Convert.ToDouble(a?.Adx),
                        Datetime = a.Datetime
                    };
                    values.Add(val);
                }

                TwelveDataAdx adx = new TwelveDataAdx()
                {
                    Symbol = jsonResponse?.Meta?.Symbol,
                    Interval = jsonResponse?.Meta?.Interval,
                    Currency = jsonResponse?.Meta?.Currency,
                    ExchangeTimezone = jsonResponse?.Meta?.ExchangeTimezone,
                    Exchange = jsonResponse?.Meta?.Exchange,
                    Type = jsonResponse?.Meta?.Type,
                    IndicatorName = jsonResponse?.Meta?.Indicator?.Name,
                    TimePeriod = jsonResponse.Meta.Indicator.TimePeriod,
                    Values = values
                };

                return adx;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("TwelveDataSharp - GetAdxAsync could not retrieve data from Twelve Data, please check your API Key and interval");
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }

        }

        /*
         * Asynchronously get Bollinger Band data for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
        public async Task<TwelveDataBollingerBands> GetBollingerBands(string symbol, string interval = "1min")
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/bbands?symbol=" + symbol + "&interval=" + interval + "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TwelveDataSharp.Models.TechnicalIndicatorBbands>(responseString);
                List<BollingerBandValue> values = new List<BollingerBandValue>();
                var bbandValues = jsonResponse.Values;
                foreach (BbandsValue b in bbandValues)
                {
                    BollingerBandValue val = new BollingerBandValue()
                    {
                        UpperBand = Convert.ToDouble(b?.UpperBand),
                        MiddleBand = Convert.ToDouble(b?.MiddleBand),
                        LowerBand = Convert.ToDouble(b?.LowerBand),
                        Datetime = b.Datetime
                    };
                    values.Add(val);
                }

                TwelveDataBollingerBands bollingerBands = new TwelveDataBollingerBands()
                {
                    Symbol = jsonResponse?.Meta?.Symbol,
                    Interval = jsonResponse?.Meta?.Interval,
                    Currency = jsonResponse?.Meta?.Currency,
                    ExchangeTimezone = jsonResponse?.Meta?.ExchangeTimezone,
                    Exchange = jsonResponse?.Meta?.Exchange,
                    Type = jsonResponse?.Meta?.Type,
                    IndicatorName = jsonResponse?.Meta?.Indicator?.Name,
                    MovingAverageType = jsonResponse?.Meta?.Indicator?.MaType,
                    StandardDeviation = jsonResponse.Meta.Indicator.Sd,
                    SeriesType = jsonResponse?.Meta?.Indicator?.SeriesType,
                    TimePeriod = jsonResponse.Meta.Indicator.TimePeriod,
                    Values = values
                };

                return bollingerBands;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("TwelveDataSharp - GetBollingerBandsAsync could not retrieve data from Twelve Data, please check your API Key and interval");
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
        }

    }
}
