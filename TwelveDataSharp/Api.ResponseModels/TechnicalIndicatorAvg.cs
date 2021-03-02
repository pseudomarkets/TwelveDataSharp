using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwelveDataSharp.Api.ResponseModels
{
    public class TechnicalIndicatorAvg
    {
        [JsonProperty("meta")]
        public AvgMeta Meta { get; set; }

        [JsonProperty("values")]
        public List<AvgValue> Values { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class AvgMeta
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("exchange_timezone")]
        public string ExchangeTimezone { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("indicator")]
        public Indicator Indicator { get; set; }
    }

    public partial class Indicator
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("series_type")]
        public string SeriesType { get; set; }

        [JsonProperty("time_period")]
        public long TimePeriod { get; set; }
    }

    public partial class AvgValue
    {
        [JsonProperty("datetime")]
        public DateTime Datetime { get; set; }

        [JsonProperty("avg")]
        public string Avg { get; set; }
    }
}
