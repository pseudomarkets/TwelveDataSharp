using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TwelveDataSharp.Models
{
    public partial class TechnicalIndicatorBbands
    {
        [JsonProperty("meta")]
        public BbandsMeta Meta { get; set; }

        [JsonProperty("values")]
        public List<BbandsValue> Values { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class BbandsMeta
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
        public BbandsIndicator Indicator { get; set; }
    }

    public partial class BbandsIndicator
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ma_type")]
        public string MaType { get; set; }

        [JsonProperty("sd")]
        public long Sd { get; set; }

        [JsonProperty("series_type")]
        public string SeriesType { get; set; }

        [JsonProperty("time_period")]
        public long TimePeriod { get; set; }
    }

    public partial class BbandsValue
    {
        [JsonProperty("datetime")]
        public DateTimeOffset Datetime { get; set; }

        [JsonProperty("upper_band")]
        public string UpperBand { get; set; }

        [JsonProperty("middle_band")]
        public string MiddleBand { get; set; }

        [JsonProperty("lower_band")]
        public string LowerBand { get; set; }
    }
}
