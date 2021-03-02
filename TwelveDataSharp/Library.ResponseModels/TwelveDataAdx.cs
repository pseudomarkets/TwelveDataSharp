using System;
using System.Collections.Generic;

namespace TwelveDataSharp.Library.ResponseModels
{
    public class TwelveDataAdx
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public string Currency { get; set; }
        public string ExchangeTimezone { get; set; }
        public string Exchange { get; set; }
        public string Type { get; set; }
        public string IndicatorName { get; set; }
        public long TimePeriod { get; set; }
        public List<AdxValues> Values { get; set; }
        public Enums.TwelveDataClientResponseStatus ResponseStatus { get; set; }
        public string ResponseMessage { get; set; } = "RESPONSE_OK";
    }

    public partial class AdxValues
    {
        public double AdxValue { get; set; }
        public DateTime Datetime { get; set; }
    }
}
