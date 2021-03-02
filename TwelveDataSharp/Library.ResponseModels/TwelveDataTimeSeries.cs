using System;
using System.Collections.Generic;

namespace TwelveDataSharp.Library.ResponseModels
{
    public class TwelveDataTimeSeries
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public string ExchangeTimezone { get; set; }
        public string Exchange { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public List<TimeSeriesValues> Values { get; set; }
        public Enums.TwelveDataClientResponseStatus ResponseStatus { get; set; }
        public string ResponseMessage { get; set; } = "RESPONSE_OK";
    }

    public partial class TimeSeriesValues
    {
        public DateTime Datetime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }
    }
}
