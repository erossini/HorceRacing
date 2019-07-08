using Amdocs.HorseRacing.Domain;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Amdocs.HorseRacing.Service
{
    internal interface IHorseRaceService
    {
        #region properties
        bool IsRaceEmpty { get; }
        bool IsRaceFull { get; }
        bool IsRaceReady { get; }
        HorseRaceState RaceState { get; }
        decimal RaceMargin { get; }
        string RaceWinner { get; }
        IReadOnlyList<Runner> RaceRunners { get; }
        long TotalRaceRuns { get; }
        bool IsRaceRunning { get; }
        IList<Horse> TopWinners { get; }

        void AddDemoRunners();

        #endregion

        #region methods
        void AddRaceRunner(string name, string oddsText);
        void ClearRaceState();
        void StartRace(int times, Action<int> action, CancellationToken token);

        #endregion
    }
}
