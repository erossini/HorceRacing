using System;
using System.Collections.Generic;
using System.Linq;

namespace Amdocs.HorseRacing.Domain
{
    public sealed class HorseRace : Entity<int>
    {
        #region properties
        public IReadOnlyList<Runner> Runners => m_Runners.ToList().AsReadOnly();
        /// <summary>
        /// To calculate the margin firstly convert each fractional odds into it’s decimal equivalent
        /// which is numerator divided by denominator and then add one. Divide 100 by each decimal equivalent
        /// and sum up all of these answers, to give the margin to 2 decimal places.
        /// Round half to even selected because this is the default rounding mode 
        /// used in IEEE 754 floating-point operations.
        /// </summary>
        public decimal Margin
        {
            get
            {
                var margin = default(decimal);
                m_Runners.ToList().ForEach(r => margin += r.OddsPrice.Margin);

                return Math.Round(margin, 2, MidpointRounding.AwayFromZero);
            }
        }
        public HorseRaceRunningState RunningState
        {
            get
            {
                var state = HorseRaceRunningState.None;

                if (m_RunningState == HorseRaceRunningState.Running)
                    state = HorseRaceRunningState.Running;

                else if (m_Runners.Count > 3 && Margin >= 110m && Margin <= 140m)
                    state = HorseRaceRunningState.Ready;

                return state;
            }
            set
            {
                m_RunningState = value;
            }
        }
        public HorseRaceState State
        {
            get
            {
                var state = HorseRaceState.Empty;

                if (m_Runners.Count > 0)
                    state = HorseRaceState.NonEmpty;

                if (m_Runners.Count == NumberOfSlots)
                    state = HorseRaceState.Full;

                return state;
            }
        }
        public Runner Winner { get; private set; } = default;

        #endregion

        #region methods
        public void AddRunner(string name, string oddsText)
        {
            AddRunner(new Runner(m_Runners.Count + 1, name, oddsText));
        }
        public void ClearRunners()
        {
            m_Runners.Clear();
        }
        public Runner FindWinner(int winner)
        {
            if (RunningState != HorseRaceRunningState.Running)
                throw new InvalidOperationException("Winning runner cannot be found. Please make sure that there are 4-16 runners in the race, and the margin is between 110% and 140%");

            var margin = default(int);

            foreach (var runner in m_Runners)
            {
                margin += (int)(runner.OddsPrice.Margin * 100);

                if (winner < margin)
                {
                    Winner = runner;
                    break;
                }
            }
            return Winner;
        }
        private void AddRunner(Runner runner)
        {
            if (State == HorseRaceState.Full)
                throw new InvalidOperationException($"Race has already {m_Runners.Count} runners. No more runners please.");

            if (m_Runners.Contains(runner))
                throw new InvalidOperationException($"Runner {runner.Identity} is already in race.");

            if (m_Runners.FirstOrDefault(r => r.Name == runner.Name) != null)
                throw new InvalidOperationException($"Horse {runner.Name} is already in race.");

            m_Runners.Add(runner);
        }

        #endregion

        #region members

        private readonly IList<Runner> m_Runners = new List<Runner>();
        private HorseRaceRunningState m_RunningState = HorseRaceRunningState.None;

        public const int NumberOfSlots = 16;

        #endregion
    }
}