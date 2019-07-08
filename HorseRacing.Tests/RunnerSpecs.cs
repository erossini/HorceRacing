using FluentAssertions;
using Xunit;

namespace Amdocs.HorseRacing.Domain.Tests
{
    public class RunnerSpecs
    {
        [Theory]
        [InlineData(1, "A BOLD MOVE", "1/2")]
        [InlineData(2, "A HARE BREATH", "1/2")]
        [InlineData(3, "A LITTLE CHAOS", "1/2")]
        [InlineData(4, "A LITTLE MAGIC", "1/2")]
        [InlineData(5, "A MOMENTOFMADNESS", "1/2")]
        [InlineData(6, "ACCORDANCE", "1/2")]
        [InlineData(7, "BIGIRONONHISHIP", "1/2")]
        [InlineData(8, "BLACK BUBLE", "1/2")]
        [InlineData(9, "CHOOCHOOBUGALOO", "1/2")]
        [InlineData(10, "DIVINE IMAGE", "1/2")]
        [InlineData(11, "DOCTOR SARDONICUS", "1/2")]
        [InlineData(12, "ENTERTAINING BEN", "1/2")]
        [InlineData(13, "FOLLOW THE SWALLOW", "1/2")]
        [InlineData(14, "GLUTNFORPUNISHMENT", "1/2")]
        [InlineData(15, "HEDIDDODINTHE", "1/2")]
        [InlineData(16, "INVIOLABLE SPIRIT", "1/2")]
        public void Check_that_runner_id_within_specification_is_accepted(int id, string name, string oddsText)
        {
            // Arrange, Act
            var runner = new Runner(id, name, oddsText);

            // Assert
            runner.Identity.Should().Be(id);
            runner.Name.Should().Be(name);
            runner.OddsPrice.Should().Be(new OddsPrice(oddsText));
        }
    }
}
