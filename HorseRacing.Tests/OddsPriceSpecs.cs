using FluentAssertions;
using System;
using Xunit;

namespace Amdocs.HorseRacing.Domain.Tests
{
    public class OddsPriceSpecs
    {
        [Theory]
        [InlineData("2/1", 2, 1)]
        [InlineData("55/6", 55, 6)]
        [InlineData("78/11", 78, 11)]
        [InlineData("167/52", 167, 52)]
        public void Check_that_odds_price_within_specification_is_accepted(string oddsText, int n, int d)
        {
            // Arrange
            // Act
            var oddsPrice = new OddsPrice(oddsText);

            // Assert
            oddsPrice.Numerator.Should().Be(n);
            oddsPrice.Denominator.Should().Be(d);
        }

        [Theory]
        [InlineData("")]
        [InlineData("/")]
        [InlineData(" /")]
        [InlineData("/ ")]
        [InlineData("1/")]
        [InlineData("/2")]
        [InlineData("1 /2")]
        [InlineData("1/ 2")]
        [InlineData("1 / 2")]
        [InlineData("a/b")]
        [InlineData("a/1")]
        [InlineData("1/b")]
        [InlineData("1.1/2")]
        [InlineData("1/1.2")]
        public void Check_that_odds_price_not_within_specification_is_not_accepted(string oddsText)
        {
            // Arrange
            var oddsPrice = default(OddsPrice);

            // Act
            Action action = () => oddsPrice = new OddsPrice(oddsText);

            // Assert
            action.Should().Throw<InvalidOperationException>();

        }

        [Theory]
        [InlineData("1/2", 66.67)]
        [InlineData("2/1", 33.33)]
        [InlineData("3/1", 25)]
        [InlineData("5/2", 28.57)]
        [InlineData("8/1", 11.11)]
        [InlineData("10/1", 9.09)]
        public void Check_that_margin_is_calculated_correctly(string oddsText, decimal expectedResult)
        {
            // Arrange, Act
            var oddsPrice = new OddsPrice(oddsText);
            var margin = Math.Round(oddsPrice.Margin, 2, MidpointRounding.ToEven);
            // Assert
            margin.Should().Be(expectedResult);
        }
    }
}
