using FluentAssertions;
using System;
using Xunit;

namespace Amdocs.HorseRacing.Domain.Tests
{
    public class HorseRaceSpecs
    {
        [Fact]
        public void Check_that_horse_race_created_empty()
        {
            // Arrange
            HorseRace horseRace;

            // Act
            horseRace = new HorseRace();

            // Assert
            horseRace.Runners.Count.Should().Be(0);
        }
        [Fact]
        public void Check_that_horse_race_state_is_empty()
        {
            // Arrange, Act
            HorseRace horseRace = new HorseRace();

            // Assert
            horseRace.State.Should().Be(HorseRaceState.Empty);
        }
        [Fact]
        public void Check_that_horse_race_state_is_non_empty()
        {
            // Arrange
            var horseRace = new HorseRace();

            // Act
            horseRace.AddRunner("A BOLD MOVE", "1/2");
            horseRace.AddRunner("A MOMENTOFMADNESS", "11/2");
            horseRace.AddRunner("HEDIDDODINTHE", "3/1");

            // Assert
            horseRace.State.Should().Be(HorseRaceState.NonEmpty);
        }
        [Fact]
        public void Check_that_horse_race_state_is_ready()
        {
            // Arrange
            var horseRace = new HorseRace();

            // Act
            horseRace.AddRunner("A BOLD MOVE", "1/2");
            horseRace.AddRunner("A MOMENTOFMADNESS", "11/2");
            horseRace.AddRunner("HEDIDDODINTHE", "3/1");
            horseRace.AddRunner("ORCHARDSTOWN CROSS", "6/1");

            // Assert
            horseRace.RunningState.Should().Be(HorseRaceRunningState.Ready);
        }
        [Fact]
        public void Check_that_horse_race_runners_are_added_as_expected()
        {
            // Arrange
            var horseRace = new HorseRace();

            // Act
            horseRace.AddRunner("A BOLD MOVE", "11/2");
            horseRace.AddRunner("A HARE BREATH", "12/1");
            horseRace.AddRunner("A LITTLE CHAOS", "13/1");
            horseRace.AddRunner("A LITTLE MAGIC", "18/1");
            horseRace.AddRunner("A MOMENTOFMADNESS", "11/2");
            horseRace.AddRunner("ACCORDANCE", "25/1");
            horseRace.AddRunner("BIGIRONONHISHIP", "13/1");
            horseRace.AddRunner("BLACK BUBLE", "18/1");
            horseRace.AddRunner("CHOOCHOOBUGALOO", "11/2");
            horseRace.AddRunner("DIVINE IMAGE", "12/1");
            horseRace.AddRunner("DOCTOR SARDONICUS", "13/1");
            horseRace.AddRunner("ENTERTAINING BEN", "18/1");
            horseRace.AddRunner("FOLLOW THE SWALLOW", "11/2");
            horseRace.AddRunner("GLUTNFORPUNISHMENT", "12/1");
            horseRace.AddRunner("HEDIDDODINTHE", "13/1");
            horseRace.AddRunner("INVIOLABLE SPIRIT", "18/1");

            // Assert
            horseRace.Runners.Count.Should().Be(16);
        }

        /// Test that for a given race, over the course of 1,000,000 iterations of calculating 
        /// the winner, the results are within 2% either way for each runner. 
        /// E.g. if a runner has a 48% chance of winning then over a 1,000,000 races 
        /// it would be expected to win somewhere between 460,000 and 500,000 times.
        [Fact]
        public void Check_that_winner_frequency_is_as_expected()
        {
            // Arrange
            int numberOfIterations = 1_000_000;

            var rng = new Random();
            var horseRace = new HorseRace();

            horseRace.AddRunner("A LITTLE CHAOS", "19/2");
            horseRace.AddRunner("A LITTLE MAGIC", "7/1");
            horseRace.AddRunner("KELLINGTON KITTY", "12/1");
            horseRace.AddRunner("KENSINGTON ART", "15/1");
            horseRace.AddRunner("KENTUCKYCONNECTION", "18/1");
            horseRace.AddRunner("LEEBELLNSUMMERBEE", "14/1");
            horseRace.AddRunner("LEHOOGG", "17/1");
            horseRace.AddRunner("LEITH HILL LEGASI", "11/1");
            horseRace.AddRunner("MAID TO REMEMBER", "13/2");
            horseRace.AddRunner("MAJESTIC APPEAL", "14/1");
            horseRace.AddRunner("MISS GARGAR", "11/2");
            horseRace.AddRunner("MISTER MUSICMASTER", "11/1");
            horseRace.AddRunner("ORANGE BLOSSOM", "17/2");
            horseRace.AddRunner("ORCHARDSTOWN CROSS", "16/1");
            horseRace.AddRunner("POETIC RHYTHM", "13/1");
            horseRace.AddRunner("POETIC ERA", "18/1");

            // Act
            horseRace.RunningState = HorseRaceRunningState.Running;

            for (int i = 0; i < numberOfIterations; i++)
            {
                var range = (int)(horseRace.Margin * 100);
                int point = rng.Next(range);

                var w = horseRace.FindWinner(point);

                w.Horse.RacesWon++;
            }

            horseRace.RunningState = HorseRaceRunningState.Ready;

            // Assert
            foreach (var r in horseRace.Runners)
            {
                var chanceOfWinning = r.OddsPrice.Margin / horseRace.Margin;
                var expectedResult = chanceOfWinning * numberOfIterations;

                r.Horse.RacesWon.Should().BeInRange((int)(expectedResult * 0.98m), (int)(expectedResult * 1.02m));
            }
        }

    }
}
