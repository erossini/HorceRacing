using Microsoft.Extensions.Hosting;

namespace Amdocs.HorseRacing.Service
{
    internal interface IHorseRaceUIService : IHostedService
    {
        void Execute(object state);
    }
}
