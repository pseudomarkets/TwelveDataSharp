using System;
using System.Collections.Generic;

namespace TwelveDataSharp.Library.ResponseModels
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
        public Enums.TwelveDataClientResponseStatus ResponseStatus { get; set; }
        public string ResponseMessage { get; set; } = "RESPONSE_OK";
    }

    public partial class BollingerBandValue
    {
        public double UpperBand { get; set; }
        public double LowerBand { get; set; }
        public double MiddleBand { get; set; }
        public DateTime Datetime { get; set; }
    }
}
