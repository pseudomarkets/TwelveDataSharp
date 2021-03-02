namespace TwelveDataSharp.Library.ResponseModels
{
    public class TwelveDataPrice
    {
        public double Price { get; set; }
        public Enums.TwelveDataClientResponseStatus ResponseStatus { get; set; }
        public string ResponseMessage { get; set; } = "RESPONSE_OK";
    }
}
