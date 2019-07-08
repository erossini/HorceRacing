using FluentAssertions;
using System;
using Xunit;

namespace Amdocs.HorseRacing.Domain.Tests
{
    public class HorseSpecs
    {
        [Theory]
        [InlineData("A")]
        [InlineData("POETIC ERA")]
        [InlineData("BIGIRONONHISHIP")]
        [InlineData("MISTER MUSICMASTER")]
        [InlineData("ABCD EFGH IJKL MNO")]
        public void Check_that_horse_name_within_specification_is_accepted(string name)
        {
            // Arrange, Act
            var horse = new Horse(name);

            // Assert
            horse.Name.Should().Be(name);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("                  ")]
        [InlineData("poetic era")]
        [InlineData("bigirononhiship")]
        [InlineData("BIGIRONONHISHIP1")]
        [InlineData("BIGIRONONHISHIP 2")]
        [InlineData("Mister Musicmaster")]
        [InlineData("ABCD EFGHIJKL(MNO)")]
        [InlineData("A LITTLE LONGER THAN IT SHOULD BE")]

        public void Check_that_horsename_not_within_specification_is_not_accepted(string name)
        {
            // Arrange
            var horse = default(Horse);

            // Act
            Action action = () => horse = new Horse(name);

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }
        [Theory]
        [InlineData("A")]
        [InlineData("POETIC ERA")]
        [InlineData("BIGIRONONHISHIP")]
        [InlineData("MISTER MUSICMASTER")]
        [InlineData("ABCD EFGH IJKL MNO")]
        public void Check_that_two_horses_with_the_same_name_are_equal(string name)
        {
            // Arrange
            var horse1 = new Horse(name);
            var horse2 = new Horse(name);

            // Act
            bool b = horse1 == horse2;

            // Assert
            b.Should().Be(true);
        }
        [Fact]
        public void Check_that_two_horses_with_different_names_are_not_equal()
        {
            // Arrange
            var horse1 = new Horse("MISTER MUSICMASTER");
            var horse2 = new Horse("BIGIRONONHISHIP");

            // Act
            bool b = horse1 == horse2;

            // Assert
            b.Should().Be(false);
        }
    }
}
