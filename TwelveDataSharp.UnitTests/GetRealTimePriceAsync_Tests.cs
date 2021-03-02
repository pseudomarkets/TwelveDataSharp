using System;
using FluentAssertions;
using Moq;
using RichardSzalay.MockHttp;
using TwelveDataSharp.Library.ResponseModels;
using Xunit;

namespace TwelveDataSharp.UnitTests
{
    public class GetRealTimePriceAsync_Tests
    {
        [Fact]
        public async void GetRealTimePrice_Success_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json", "{\"price\":\"127.79000\"}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetRealTimePriceAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.Ok);
            response?.ResponseMessage.Should().Be("RESPONSE_OK");
            response?.Price.Should().Be(127.7900);
        }

        [Fact]
        public async void GetRealTimePrice_BadApiKey_Test()
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
        public async void GetRealTimePrice_InvalidSymbol_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json",
                    "{\"code\":400,\"message\":\"**symbol** not found: FAKE. Please specify it correctly according to API Documentation.\",\"status\":\"error\",\"meta\":{\"symbol\":\"FAKE\",\"interval\":\"\",\"exchange\":\"\"}}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetRealTimePriceAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataApiError);
        }

        [Fact]
        public async void GetRealTimePrice_NullHttpClient_Test()
        {
            // Arrange
            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", null);

            // Act
            var response = await twelveDataClient.GetRealTimePriceAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataSharpError);
        }
    }
}
