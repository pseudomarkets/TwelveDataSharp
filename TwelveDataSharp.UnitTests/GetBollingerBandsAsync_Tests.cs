using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using RichardSzalay.MockHttp;
using TwelveDataSharp.Library.ResponseModels;
using Xunit;

namespace TwelveDataSharp.UnitTests
{
    public class GetBollingerBandsAsync_Tests
    {
        [Fact]
        public async void GetBollingerBandsAsync_Success_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json", "{\"meta\":{\"symbol\":\"AAPL\",\"interval\":\"1min\",\"currency\":\"USD\",\"exchange_timezone\":\"America/New_York\",\"exchange\":\"NASDAQ\",\"type\":\"Common Stock\",\"indicator\":{\"name\":\"BBANDS - Bollinger Bands®\",\"series_type\":\"close\",\"time_period\":20,\"sd\":2,\"ma_type\":\"SMA\"}},\"values\":[{\"datetime\":\"2021-03-01 15:59:00\",\"upper_band\":\"127.80482\",\"middle_band\":\"127.55686\",\"lower_band\":\"127.30890\"}],\"status\":\"ok\"}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetBollingerBandsAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.Ok);
            response?.ResponseMessage.Should().Be("RESPONSE_OK");
            response?.Symbol?.Should().Be("AAPL");
            response?.Interval.Should().Be("1min");
            response?.Currency.Should().Be("USD");
            response?.ExchangeTimezone.Should().Be("America/New_York");
            response?.Exchange.Should().Be("NASDAQ");
            response?.Type.Should().Be("Common Stock");
            response?.MovingAverageType.Should().Be("SMA");
            response?.IndicatorName.Should().Be("BBANDS - Bollinger Bands®");
            response?.SeriesType.Should().Be("close");
            response?.TimePeriod.Should().Be(20);
            response?.StandardDeviation.Should().Be(2);
            response?.Values[0]?.Datetime.Should().Be(new DateTime(2021, 3, 1, 15, 59, 0));
            response?.Values[0]?.LowerBand.Should().Be(127.30890);
            response?.Values[0]?.MiddleBand.Should().Be(127.55686);
            response?.Values[0].UpperBand.Should().Be(127.80482);
        }

        [Fact]
        public async void GetBollingerBandsAsync_BadApiKey_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json",
                    "{\"code\":401,\"message\":\"**apikey** parameter is incorrect or not specified. You can get your free API Key instantly following this link: https://twelvedata.com/apikey. If you believe that everything is correct, you can email us at apikey@twelvedata.com\",\"status\":\"error\"}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetBollingerBandsAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataApiError);
        }

        [Fact]
        public async void GetBollingerBandsAsync_InvalidSymbol_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json",
                    "{\"code\":400,\"message\":\"**symbol** not found: FAKE. Please specify it correctly according to API Documentation.\",\"status\":\"error\",\"meta\":{\"symbol\":\"FAKE\",\"interval\":\"\",\"exchange\":\"\"}}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetBollingerBandsAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataApiError);
        }

        [Fact]
        public async void GetBollingerBandsAsync_NullHttpClient_Test()
        {
            // Arrange
            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", null);

            // Act
            var response = await twelveDataClient.GetBollingerBandsAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataSharpError);
        }
    }
}
