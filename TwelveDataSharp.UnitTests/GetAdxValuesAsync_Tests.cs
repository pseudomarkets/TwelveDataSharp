using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using RichardSzalay.MockHttp;
using TwelveDataSharp.Library.ResponseModels;
using Xunit;

namespace TwelveDataSharp.UnitTests
{
    public class GetAdxValuesAsync_Tests
    {
        [Fact]
        public async void GetAdxValuesAsync_Success_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json", "{\"meta\":{\"symbol\":\"AAPL\",\"interval\":\"1min\",\"currency\":\"USD\",\"exchange_timezone\":\"America/New_York\",\"exchange\":\"NASDAQ\",\"type\":\"Common Stock\",\"indicator\":{\"name\":\"ADX - Average Directional Index\",\"time_period\":14}},\"values\":[{\"datetime\":\"2021-03-01 15:59:00\",\"adx\":\"15.69773\"}],\"status\":\"ok\"}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetAdxValuesAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.Ok);
            response?.ResponseMessage.Should().Be("RESPONSE_OK");
            response?.Symbol.Should().Be("AAPL");
            response?.Interval.Should().Be("1min");
            response?.Currency.Should().Be("USD");
            response?.ExchangeTimezone.Should().Be("America/New_York");
            response?.Exchange.Should().Be("NASDAQ");
            response?.Type.Should().Be("Common Stock");
            response?.IndicatorName.Should().Be("ADX - Average Directional Index");
            response?.TimePeriod.Should().Be(14);
            response?.Values[0]?.AdxValue.Should().Be(15.69773);
            response?.Values[0]?.Datetime.Should().Be(new DateTime(2021, 3, 1, 15, 59, 0));
        }

        [Fact]
        public async void GetAdxValuesAsync_BadApiKey_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json",
                    "{\"code\":401,\"message\":\"**apikey** parameter is incorrect or not specified. You can get your free API Key instantly following this link: https://twelvedata.com/apikey. If you believe that everything is correct, you can email us at apikey@twelvedata.com\",\"status\":\"error\"}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetRealTimePriceAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataApiError);
        }

        [Fact]
        public async void GetAdxValuesAsync_InvalidSymbol_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json",
                    "{\"code\":400,\"message\":\"**symbol** not found: FAKE. Please specify it correctly according to API Documentation.\",\"status\":\"error\",\"meta\":{\"symbol\":\"FAKE\",\"interval\":\"\",\"exchange\":\"\"}}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetAdxValuesAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataApiError);
        }

        [Fact]
        public async void GetAdxValuesAsync_NullHttpClient_Test()
        {
            // Arrange
            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", null);

            // Act
            var response = await twelveDataClient.GetAdxValuesAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataSharpError);
        }
    }
}
