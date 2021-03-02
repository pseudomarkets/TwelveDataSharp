using System;
using Newtonsoft.Json;

namespace TwelveDataSharp.Api.ResponseModels
{
    class TimeSeriesQuote
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

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

        [JsonProperty("previous_close")]
        public string PreviousClose { get; set; }

        [JsonProperty("change")]
        public string Change { get; set; }

        [JsonProperty("percent_change")]
        public string PercentChange { get; set; }

        [JsonProperty("average_volume")]
        public long AverageVolume { get; set; }

        [JsonProperty("fifty_two_week")]
        public FiftyTwoWeek FiftyTwoWeek { get; set; }
    }

    public partial class FiftyTwoWeek
    {
        [JsonProperty("low")]
        public string Low { get; set; }

        [JsonProperty("high")]
        public string High { get; set; }

        [JsonProperty("low_change")]
        public string LowChange { get; set; }

        [JsonProperty("high_change")]
        public string HighChange { get; set; }

        [JsonProperty("low_change_percent")]
        public string LowChangePercent { get; set; }

        [JsonProperty("high_change_percent")]
        public string HighChangePercent { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }
    }
}
