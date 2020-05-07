using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TwelveDataSharp.Models
{
    public class TimeSeriesRealTimePrice
    {
        [JsonProperty("price")]
        public string Price { get; set; }
    }
}
