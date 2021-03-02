using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwelveDataSharp.Api.ResponseModels
{
    public partial class TechnicalIndicatorAdx
    {
        [JsonProperty("meta")]
        public AdxMeta Meta { get; set; }

        [JsonProperty("values")]
        public List<AdxValue> Values { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class AdxMeta
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
        public AdxIndicator Indicator { get; set; }
    }

    public partial class AdxIndicator
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time_period")]
        public long TimePeriod { get; set; }
    }

    public partial class AdxValue
    {
        [JsonProperty("datetime")]
        public DateTime Datetime { get; set; }

        [JsonProperty("adx")]
        public string Adx { get; set; }
    }
}
