using Amdocs.HorseRacing.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Amdocs.HorseRacing.Service
{
    public class HorseRaceService : IHorseRaceService
    {
        #region properties
        public IReadOnlyList<Runner> RaceRunners => m_HorseRace.Runners;
        public Domain.HorseRaceState RaceState => m_HorseRace.State;
        public decimal RaceMargin => m_HorseRace.Margin;
        public bool IsRaceFull => m_HorseRace.State == HorseRaceState.Full;
        public bool IsRaceEmpty => m_HorseRace.State == HorseRaceState.Empty;
        public bool IsRaceReady => m_HorseRace.RunningState == HorseRaceRunningState.Ready;
        public bool IsRaceRunning => m_HorseRace.RunningState == HorseRaceRunningState.Running;
        public IList<Horse> TopWinners
        {
            get
            {
                var racesWon = default(long);
                var topRunners = new List<Horse>();

                foreach (var r in RaceRunners)
                    if (r.Horse.RacesWon > 0)
                        if (r.Horse.RacesWon >= racesWon)
                            racesWon = r.Horse.RacesWon;

                foreach (var r in RaceRunners)
                    if (r.Horse.RacesWon > 0)
                        if (r.Horse.RacesWon == racesWon)
                            topRunners.Add(r.Horse);

                return topRunners;
            }
        }
        public string RaceWinner
        {
            get
            {
                var currentWinner = "None";

                if (m_HorseRace.Winner != null)
                    currentWinner = m_HorseRace.Winner.ToString();

                return currentWinner;
            }
        }
        public long TotalRaceRuns { get; private set; } = default;

        #endregion

        #region methods

        public void AddRaceRunner(string name, string oddsText)
        {
            m_HorseRace.AddRunner(name, oddsText);
            m_Logger.LogDebug($"Runner {name} added to race with odds {oddsText}.");
        }
        public void ClearRaceState()
        {
            TotalRaceRuns = 0;

            m_HorseRace.ClearRunners();
            m_Logger.LogDebug($"Race state cleared.");
        }
        public void StartRace(int times, Action<int> action, CancellationToken token)
        {
            m_HorseRace.RunningState = Domain.HorseRaceRunningState.Running;

            for (var i = 0; i < times; i++)
            {
                int point = m_Rng.Next((int)(m_HorseRace.Margin * 100));

                var w = m_HorseRace.FindWinner(point);
                // m_Logger.LogDebug($"Point {point} in range {m_HorseRace.Margin * 100}. Found winner: {w}.");

                // Notify caller
                if (i % 10000 == 0)
                    action(i);

                TotalRaceRuns++;
                w.Horse.RacesWon++;

                if (token.IsCancellationRequested)
                    break;
            }
            m_HorseRace.RunningState = Domain.HorseRaceRunningState.Ready;
            action(times);
        }
        public void AddDemoRunners()
        {
            ClearRaceState();

            AddRaceRunner("A BOLD MOVE", "11/2");
            AddRaceRunner("A HARE BREATH", "12/1");
            AddRaceRunner("A LITTLE CHAOS", "13/1");
            AddRaceRunner("A LITTLE MAGIC", "18/1");
            AddRaceRunner("A MOMENTOFMADNESS", "9/1");
            AddRaceRunner("ACCORDANCE", "25/1");
            AddRaceRunner("BIGIRONONHISHIP", "14/1");
            AddRaceRunner("BLACK BUBLE", "18/1");
            AddRaceRunner("CHOOCHOOBUGALOO", "10/2");
            AddRaceRunner("DIVINE IMAGE", "12/1");
            AddRaceRunner("DOCTOR SARDONICUS", "21/1");
            AddRaceRunner("ENTERTAINING BEN", "19/1");
            AddRaceRunner("FOLLOW THE SWALLOW", "18/2");
            AddRaceRunner("GLUTNFORPUNISHMENT", "13/2");
            AddRaceRunner("HEDIDDODINTHE", "15/1");
            AddRaceRunner("INVIOLABLE SPIRIT", "27/2");

            m_Logger.LogDebug($"Demo Runners added to race.");
        }

        #endregion

        #region constructors
        public HorseRaceService(HorseRace horseRace, Random rng, ILogger<HorseRaceService> logger)
        {
            m_Rng = rng ?? throw new InvalidOperationException("Cannot utilize Random Number Generator.");
            m_HorseRace = horseRace ?? throw new InvalidOperationException("A horse race cannot be initiated.");
            m_Logger = logger;
        }

        #endregion

        #region members

        private readonly Random m_Rng;
        private readonly HorseRace m_HorseRace;
        private readonly ILogger<HorseRaceService> m_Logger;

        #endregion
    }
}
