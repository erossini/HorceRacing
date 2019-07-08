using Amdocs.HorseRacing.Domain;
using Amdocs.HorseRacing.Service;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Amdocs.HorseRacing.Console.UI;

namespace Amdocs.HorseRacing.Console
{
    /// <summary>
    /// Console application that simulates and displays the result of a horse race.
    /// </summary>
    internal class Program
    {
        #region methods

        private static async Task Main()
        {
            System.Console.Title = "Amdocs Horseracing Simulator";

            var builder = new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices(services => services.AddAutofac())  //  Adds services to the Dependency Injection container
                .ConfigureContainer<ContainerBuilder>((context, containerBuilder) => // Configure the instantiated dependency injection container
                {
                    containerBuilder
                        .RegisterType<Random>()
                        .SingleInstance();

                    containerBuilder
                        .RegisterType<SimpleConsoleWriter>()
                        .As<IConsoleWriter>();

                    containerBuilder
                        .RegisterType<SimpleConsoleReader>()
                        .As<IConsoleReader>();

                    containerBuilder
                        .RegisterType<HorseRace>();

                    containerBuilder
                        .RegisterType<HorseRaceService>()
                        .As<IHorseRaceService>();

                    containerBuilder
                        .RegisterType<HorseRaceUIService>()
                        .As<IHostedService>()
                        .SingleInstance();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration
                        .MinimumLevel.Debug()
                        .WriteTo.File("log.txt",
                            retainedFileCountLimit: 10,
                            fileSizeLimitBytes: 8388608, 
                            rollOnFileSizeLimit: true, 
                            rollingInterval: RollingInterval.Day))
                .UseConsoleLifetime();

            await builder.RunConsoleAsync();
        }
    }
    #endregion
}