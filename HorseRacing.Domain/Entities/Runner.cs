using System;

namespace Amdocs.HorseRacing.Domain
{
    /// <summary>
    /// A Runner class is an identifiable entity, combining a horse and an odds price.
    /// A runner can participate in a Horse Race.
    /// </summary>
    public sealed class Runner : Entity<int>
    {
        #region properties
        public Horse Horse { get; }
        public string Name => Horse.Name;
        public OddsPrice OddsPrice { get; }

        #endregion

        #region constructors
        public Runner(int id, string name, string oddsText)
            : this(id, new Horse(name), new OddsPrice(oddsText))
        {
        }
        private Runner(int id, Horse horse, OddsPrice odds)
        {
            Horse = horse ?? throw new InvalidOperationException("Runner needs a horse instance");
            OddsPrice = odds ?? throw new InvalidOperationException("Runner needs odds price.");

            Identity = id;
        }
        #endregion

        #region methods
        public decimal ChanceOfWinning(decimal raceMargin)
        {
            if (raceMargin == 0m)
                throw new InvalidOperationException("Cannot calculate chance of winning if overall race margin is 0.");

            return Math.Round((100m * OddsPrice.Margin) / raceMargin, 2, MidpointRounding.ToEven);
        }
        public override string ToString()
        {
            return $"{Identity,2}.{Horse}";
        }

        #endregion
    }
}