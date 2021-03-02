using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwelveDataSharp.Api.ResponseModels;
using TwelveDataSharp.Interfaces;
using TwelveDataSharp.Library.ResponseModels;

/*
 * TwelveDataSharp - a .NET Standard 2.0 library for accessing stock market data from Twelve Data
 * Author - Shravan Jambukesan <shravan@shravanj.com>
 * https://github.com/pseudomarkets/TwelveDataSharp
 */

namespace TwelveDataSharp
{
    public class TwelveDataClient : ITwelveDataClient
    {
        private readonly string _apiKey;
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
                string endpoint = "https://api.twelvedata.com/quote?symbol=" + symbol + "&interval=" + interval +
                                  "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TimeSeriesQuote>(responseString);
                TwelveDataQuote quote = new TwelveDataQuote()
                {
                    Symbol = jsonResponse?.Symbol,
                    Name = jsonResponse?.Name,
                    Exchange = jsonResponse?.Exchange,
                    Currency = jsonResponse?.Currency,
                    Datetime = jsonResponse?.Datetime ?? DateTime.MinValue,
                    Open = Convert.ToDouble(jsonResponse?.Open),
                    Close = Convert.ToDouble(jsonResponse?.Close),
                    High = Convert.ToDouble(jsonResponse?.High),
                    Low = Convert.ToDouble(jsonResponse?.Low),
                    Volume = jsonResponse?.Volume ?? 0,
                    PreviousClose = Convert.ToDouble(jsonResponse?.PreviousClose),
                    Change = Convert.ToDouble(jsonResponse?.Change),
                    PercentChange = Convert.ToDouble(jsonResponse?.PercentChange),
                    AverageVolume = jsonResponse?.AverageVolume ?? 0,
                    FiftyTwoWeekHigh = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.High),
                    FiftyTwoWeekLow = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.Low),
                    FiftyTwoWeekHighChange = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.HighChange),
                    FiftyTwoWeekLowChange = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.LowChange),
                    FiftyTwoWeekHighChangePercent = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.HighChangePercent),
                    FiftyTwoWeekLowChangePercent = Convert.ToDouble(jsonResponse?.FiftyTwoWeek?.LowChangePercent),
                    FiftyTwoWeekRange = jsonResponse?.FiftyTwoWeek?.Range
                };

                if (!string.IsNullOrEmpty(quote.Symbol) && quote.Datetime != DateTime.MinValue) 
                    return quote;
                quote.ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataApiError;
                quote.ResponseMessage = "Invalid symbol or bad API key";

                return quote;
            }
            catch (Exception e)
            {
                return new TwelveDataQuote()
                {
                    ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataSharpError,
                    ResponseMessage = e.ToString()
                };
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
                var jsonResponse = JsonConvert.DeserializeObject<TimeSeriesRealTimePrice>(responseString);
                TwelveDataPrice realTimePrice = new TwelveDataPrice()
                {
                    Price = Convert.ToDouble(jsonResponse?.Price)
                };

                if (!realTimePrice.Price.Equals(0)) return realTimePrice;
                realTimePrice.ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataApiError;
                realTimePrice.ResponseMessage = "Invalid symbol or bad API key";

                return realTimePrice;
            }
            catch (Exception e)
            {
                return new TwelveDataPrice()
                {
                    ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataSharpError,
                    ResponseMessage = e.ToString()
                };
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
                string endpoint = "https://api.twelvedata.com/time_series?symbol=" + symbol + "&interval=" + interval +
                                  "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TimeSeriesStocks>(responseString);
                List<TimeSeriesValues> values = new List<TimeSeriesValues>();
                var tsStockValues = jsonResponse?.Values;
                if (tsStockValues != null)
                {
                    values.AddRange(tsStockValues.Select(v => new TimeSeriesValues()
                    {
                        Datetime = v?.Datetime ?? DateTime.MinValue,
                        Open = Convert.ToDouble(v?.Open),
                        High = Convert.ToDouble(v?.High),
                        Low = Convert.ToDouble(v?.Low),
                        Close = Convert.ToDouble(v?.Close),
                        Volume = v.Volume
                    }));
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

                if (!string.IsNullOrEmpty(timeSeries?.Symbol) && values.Count != 0) 
                    return timeSeries;
                timeSeries.ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataApiError;
                timeSeries.ResponseMessage = "Invalid symbol or API key";

                return timeSeries;
            }
            catch (Exception e)
            {
                return new TwelveDataTimeSeries()
                {
                    ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataSharpError,
                    ResponseMessage = e.ToString()
                };
            }
        }

        /*
         * Asynchronously get time series averages for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
        public async Task<TwelveDataTimeSeriesAverage> GetTimeSeriesAverageAsync(string symbol,
            string interval = "1min")
        {
            try
            {
                var client = new HttpClient();
                string endpoint = "https://api.twelvedata.com/avg?symbol=" + symbol + "&interval=" + interval +
                                  "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TechnicalIndicatorAvg>(responseString);
                List<TimeSeriesAverages> values = new List<TimeSeriesAverages>();
                var tsAverageValues = jsonResponse?.Values;

                if (tsAverageValues != null)
                {
                    values.AddRange(tsAverageValues.Select(v => new TimeSeriesAverages()
                        {AverageValue = Convert.ToDouble(v?.Avg), Datetime = v?.Datetime ?? DateTime.MinValue}));
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
                    TimePeriod = jsonResponse?.Meta?.Indicator?.TimePeriod ?? 0,
                    Values = values
                };

                if (!string.IsNullOrEmpty(timeSeriesAverage.Symbol) && values.Count != 0) 
                    return timeSeriesAverage;
                timeSeriesAverage.ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataApiError;
                timeSeriesAverage.ResponseMessage = "Invalid symbol or API key";

                return timeSeriesAverage;
            }
            catch (Exception e)
            {
                return new TwelveDataTimeSeriesAverage()
                {
                    ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataSharpError,
                    ResponseMessage = e.ToString()
                };
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
                string endpoint = "https://api.twelvedata.com/adx?symbol=" + symbol + "&interval=" + interval +
                                  "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TechnicalIndicatorAdx>(responseString);
                List<AdxValues> values = new List<AdxValues>();
                var adxValues = jsonResponse?.Values;

                if (adxValues != null)
                {
                    values.AddRange(adxValues.Select(a => new AdxValues()
                        {AdxValue = Convert.ToDouble(a?.Adx), Datetime = a?.Datetime ?? DateTime.MinValue}));
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
                    TimePeriod = jsonResponse?.Meta?.Indicator?.TimePeriod ?? 0,
                    Values = values
                };

                if (!string.IsNullOrEmpty(adx.Symbol) && adx.Values.Count != 0) 
                    return adx;
                adx.ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataApiError;
                adx.ResponseMessage = "Invalid symbol or API key";

                return adx;
            }
            catch (Exception e)
            {
                return new TwelveDataAdx()
                {
                    ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataSharpError,
                    ResponseMessage = e.ToString()
                };
            }
        }

        /*
         * Asynchronously get Bollinger Band data for a particular NASDAQ or NYSE listed equity or ETF with a specified interval
         * symbol - a valid symbol for an equity or ETF listed on the NASDAQ or NYSE
         * interval - supports intervals "1min", "5min", "15min", "30min", "45min", "1h", "2h", "4h", "1day", "1week", and "1month"
         * with a default value set to "1min"
         */
        public async Task<TwelveDataBollingerBands> GetBollingerBandsAsync(string symbol, string interval = "1min")
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/bbands?symbol=" + symbol + "&interval=" + interval +
                                  "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TechnicalIndicatorBbands>(responseString);
                List<BollingerBandValue> values = new List<BollingerBandValue>();
                var bbandValues = jsonResponse?.Values;

                if (bbandValues != null)
                {
                    values.AddRange(bbandValues.Select(b => new BollingerBandValue()
                    {
                        UpperBand = Convert.ToDouble(b?.UpperBand), MiddleBand = Convert.ToDouble(b?.MiddleBand),
                        LowerBand = Convert.ToDouble(b?.LowerBand), Datetime = b?.Datetime ?? DateTime.MinValue
                    }));
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
                    StandardDeviation = jsonResponse?.Meta?.Indicator?.Sd ?? 0,
                    SeriesType = jsonResponse?.Meta?.Indicator?.SeriesType,
                    TimePeriod = jsonResponse?.Meta?.Indicator?.TimePeriod ?? 0,
                    Values = values
                };

                if (!string.IsNullOrEmpty(bollingerBands.Symbol) && bollingerBands.Values.Count != 0)
                    return bollingerBands;
                bollingerBands.ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataApiError;
                bollingerBands.ResponseMessage = "Invalid symbol or API key";

                return bollingerBands;
            }
            catch (Exception e)
            {
                return new TwelveDataBollingerBands()
                {
                    ResponseStatus = Enums.TwelveDataClientResponseStatus.TwelveDataSharpError,
                    ResponseMessage = e.ToString()
                };
            }
        }
    }
}
