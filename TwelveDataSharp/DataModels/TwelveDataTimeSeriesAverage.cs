using System;
using System.Collections.Generic;
using System.Text;

namespace TwelveDataSharp.DataModels
{
    public class TwelveDataTimeSeriesAverage
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public string Currency { get; set; }
        public string ExchangeTimezone { get; set; }
        public string Exchange { get; set; }
        public string Type { get; set; }
        public string IndicatorName { get; set; }
        public string SeriesType { get; set; }
        public long TimePeriod { get; set; }
        public List<TimeSeriesAverages> Values { get; set; }
    }

    public partial class TimeSeriesAverages
    {
        public double AverageValue { get; set; }
        public DateTimeOffset Datetime { get; set; }
    }
}
