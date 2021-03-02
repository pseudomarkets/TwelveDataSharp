using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwelveDataSharp.Api.ResponseModels
{
    public class TimeSeriesStocks
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("values")]
        public List<Value> Values { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Meta
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
    }

    public partial class Value
    {
        [JsonProperty("datetime")]
        public DateTime Datetime { get; set; }

        [JsonProperty("open")]
        public string Open { get; set; }

        [JsonProperty("high")]
        public string High { get; set; }

        [JsonProperty("low")]
        public string Low { get; set; }

        [JsonProperty("close")]
        public string Close { get; set; }

        [JsonProperty("volume")]
        public long Volume { get; set; }
    }
}
