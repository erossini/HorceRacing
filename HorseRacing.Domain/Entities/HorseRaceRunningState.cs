using System;

namespace Amdocs.HorseRacing.Domain
{
    [Flags]
    public enum HorseRaceRunningState
    {
        None,
        Ready,
        Running
        
    }
}