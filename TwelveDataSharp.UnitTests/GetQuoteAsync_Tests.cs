using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using RichardSzalay.MockHttp;
using TwelveDataSharp.Library.ResponseModels;
using Xunit;

namespace TwelveDataSharp.UnitTests
{
    public class GetQuoteAsync_Tests
    {
        [Fact]
        public async void GetQuote_Success_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json",
                    "{\"symbol\":\"AAPL\",\"name\":\"Apple Inc\",\"exchange\":\"NASDAQ\",\"currency\":\"USD\",\"datetime\":\"2021-03-01\",\"open\":\"123.75000\",\"high\":\"127.93000\",\"low\":\"122.79000\",\"close\":\"127.79000\",\"volume\":\"116307692\",\"previous_close\":\"121.26000\",\"change\":\"6.53000\",\"percent_change\":\"5.38512\",\"average_volume\":\"115516862\",\"fifty_two_week\":{\"low\":\"53.15250\",\"high\":\"145.08000\",\"low_change\":\"74.63750\",\"high_change\":\"-17.29000\",\"low_change_percent\":\"140.42143\",\"high_change_percent\":\"-11.91756\",\"range\":\"53.152500 - 145.080002\"}}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetQuoteAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.Ok);
            response?.ResponseMessage.Should().Be("RESPONSE_OK");
            response?.Symbol.Should().Be("AAPL");
            response?.Open.Should().Be(123.75000);
            response?.Datetime.Should().Be(new DateTime(2021, 3, 1));
            response?.Exchange.Should().Be("NASDAQ");
            response?.Currency.Should().Be("USD");
            response?.Datetime.Should().Be(new DateTime(2021, 3, 1));
            response?.Open.Should().Be(123.75000);
            response?.High.Should().Be(127.93000);
            response?.Low.Should().Be(122.79000);
            response?.Close.Should().Be(127.79000);
            response?.PreviousClose.Should().Be(121.26000);
            response?.Change.Should().Be(6.53000);
            response?.Volume.Should().Be(116307692);
            response?.Change.Should().Be(6.53000);
            response?.PercentChange.Should().Be(5.38512);
            response?.AverageVolume.Should().Be(115516862);
            response?.FiftyTwoWeekHigh.Should().Be(145.08000);
            response?.FiftyTwoWeekLow.Should().Be(53.15250);
            response?.FiftyTwoWeekHighChange.Should().Be(-17.29000);
            response?.FiftyTwoWeekLowChange.Should().Be(74.63750);
            response?.FiftyTwoWeekHighChangePercent.Should().Be(-11.91756);
            response?.FiftyTwoWeekLowChangePercent.Should().Be(140.42143);
            response?.FiftyTwoWeekRange.Should().Be("53.152500 - 145.080002");
        }

        [Fact]
        public async void GetQuote_InvalidSymbol_Test()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When("https://api.twelvedata.com/*")
                .Respond("application/json",
                    "{\"code\":400,\"message\":\"**symbol** not found: FAKE. Please specify it correctly according to API Documentation.\",\"status\":\"error\",\"meta\":{\"symbol\":\"FAKE\",\"interval\":\"\",\"exchange\":\"\"}}");

            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", mockHttp.ToHttpClient());

            // Act
            var response = await twelveDataClient.GetQuoteAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataApiError);
        }

        [Fact]
        public async void GetQuote_BadApiKey_Test()
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
        public async void GetQuote_NullHttpClient_Test()
        {
            // Arrange
            TwelveDataClient twelveDataClient = new TwelveDataClient("TEST", null);

            // Act
            var response = await twelveDataClient.GetQuoteAsync("TEST");

            // Assert
            response?.ResponseStatus.Should().Be(Enums.TwelveDataClientResponseStatus.TwelveDataSharpError);
        }
    }
}
