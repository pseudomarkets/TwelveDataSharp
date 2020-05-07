using System;
using System.Collections.Generic;
using System.Text;

namespace TwelveDataSharp.DataModels
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
    }

    public partial class TimeSeriesValues
    {
        public DateTimeOffset Datetime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }
    }
}
