using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TwelveDataSharp.Models
{
    public class TechnicalIndicatorAvg
    {
        [JsonProperty("meta")]
        public TIMeta Meta { get; set; }

        [JsonProperty("values")]
        public List<TIValue> Values { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class TIMeta
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

    public partial class TIValue
    {
        [JsonProperty("datetime")]
        public DateTimeOffset Datetime { get; set; }

        [JsonProperty("avg")]
        public string Avg { get; set; }
    }
}
