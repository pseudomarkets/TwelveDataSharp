using System;

namespace TwelveDataSharp.Library.ResponseModels
{
    public class TwelveDataQuote
    {
        public string Symbol { get; set; }
        public string Name { get; set; } 
        public string Exchange { get; set; }
        public string Currency { get; set; }
        public DateTime Datetime { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public long Volume { get; set; }
        public double PreviousClose { get; set; }
        public double Change { get; set; }
        public double PercentChange { get; set; }
        public long AverageVolume { get; set; }
        public double FiftyTwoWeekLow { get; set; }
        public double FiftyTwoWeekHigh { get; set; }
        public double FiftyTwoWeekHighChange { get; set; }
        public double FiftyTwoWeekLowChange { get; set; }
        public double FiftyTwoWeekHighChangePercent { get; set; }
        public double FiftyTwoWeekLowChangePercent { get; set; }
        public string FiftyTwoWeekRange { get; set; }
        public Enums.TwelveDataClientResponseStatus ResponseStatus { get; set; }
        public string ResponseMessage { get; set; } = "RESPONSE_OK";
    }
}
