using System;
using System.Collections.Generic;
using System.Text;

namespace TwelveDataSharp.DataModels
{
    public class TwelveDataBollingerBands
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public string Currency { get; set; }
        public string ExchangeTimezone { get; set; }
        public string Exchange { get; set; }
        public string Type { get; set; }
        public string IndicatorName { get; set; }
        public string MovingAverageType { get; set; }
        public long StandardDeviation { get; set; }
        public string SeriesType { get; set; }
        public long TimePeriod { get; set; }
        public List<BollingerBandValue> Values { get; set; } 
    }

    public partial class BollingerBandValue
    {
        public double UpperBand { get; set; }
        public double LowerBand { get; set; }
        public double MiddleBand { get; set; }
        public DateTimeOffset Datetime { get; set; }
    }
}
