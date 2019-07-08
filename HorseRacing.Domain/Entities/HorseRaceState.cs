using System;

namespace Amdocs.HorseRacing.Domain
{
    [Flags]
    public enum HorseRaceState
    {
        Empty,
        NonEmpty,
        Full
    }
}