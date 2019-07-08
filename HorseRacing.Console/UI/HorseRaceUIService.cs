using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amdocs.HorseRacing.Console.Infrastructure;
using Amdocs.HorseRacing.Console.UI;
using Amdocs.HorseRacing.Domain;
using Microsoft.Extensions.Logging;

namespace Amdocs.HorseRacing.Service
{
    internal class HorseRaceUIService : IHorseRaceUIService, IDisposable
    {
        #region properties
        internal string Title => "Horse Race Simulator";

        // Views
        private (int Top, int Left, int Height, int Width) View => (1, 1, 42, 90);
        private (int Top, int Left, int Height, int Width) TitleView => (1, 1, 1, 88);
        private (int Top, int Left, int Height, int Width) RaceSummaryView => (4, 1, 50, 88);
        private (int Top, int Left, int Height, int Width) RaceRunnersView => (11, 1, 50, 88);
        private (int Top, int Left, int Height, int Width) MainMenuView => (29, 1, 50, 88);
        private (int Top, int Left, int Height, int Width) AddRunnerView => (4, 1, 20, 88);

        #endregion

        #region methods

        public void Execute(object state)
        {
            do
            {
                ClearConsole();

                ShowTitle(TitleView, Title);
                ShowRace();
                ShowMenu(MainMenuView);
                ShowMessages();

                var k = m_InputReader.ReadKey(true);

                m_Messages.Clear();
                ProcessKey(k);

            } while (true);
        }
        /// <summary>
        /// Show the application title
        /// </summary>
        /// <param name="top">Top row</param>
        /// <param name="left">Left Column</param>
        /// <param name="top">Offset from top</param>
        /// <returns></returns>
        private void ShowTitle((int Top, int Left, int Height, int Width) view, string text)
        {
            m_UI.Center(view.Top, view.Left, text, ConsoleColor.Yellow, ConsoleColor.Blue, view.Width);
        }
        private void ShowRace()
        {
            m_UI.CursorVisibility = false;

            ShowRaceSummary(RaceSummaryView);
            ShowRunners(RaceRunnersView);
        }
        private void ShowRaceSummary((int Top, int Left, int Height, int Width) view)
        {
            int height = 0;

            m_UI.Write(view.Top + height++, view.Left, "Race summary:", ConsoleColor.Yellow);

            m_UI.Write(view.Top + height++, view.Left, $"  The horse race has currently {((m_HorseRaceService.RaceRunners.Count == 0) ? "no" : m_HorseRaceService.RaceRunners.Count.ToString())} runners. ", ConsoleColor.White);
            m_UI.Write(view.Top + height++, view.Left, $"  Current Race Margin is: {m_HorseRaceService.RaceMargin:N2}");
            m_UI.Write(view.Top + height++, view.Left, $"  Current Race state is: {m_HorseRaceService.RaceState}");
            m_UI.Write(view.Top + height++, view.Left, $"  Total Runs: {m_HorseRaceService.TotalRaceRuns:N0}");

            m_UI.Write(view.Top + height, view.Left, "  Last Race Winner is: ");
            m_UI.Write(view.Top + height++, view.Left + 23, $"{m_HorseRaceService.RaceWinner}", ConsoleColor.Green, ConsoleColor.DarkBlue);
        }
        private void ShowRunners((int Top, int Left, int Height, int Width) view)
        {
            int height = 0;

            if (m_HorseRaceService.IsRaceEmpty)
            {
                m_UI.Left(view.Top + height, view.Left, "  There are currently no runners in the race.", ConsoleColor.DarkBlue, ConsoleColor.Yellow, view.Width);
            }
            else
            {
                m_UI.Center(view.Top + height, view.Left, $"Runner", ConsoleColor.DarkBlue, ConsoleColor.Yellow, 24);
                m_UI.Center(view.Top + height, view.Left + 24, "Odds", ConsoleColor.DarkBlue, ConsoleColor.Yellow, 12);
                m_UI.Center(view.Top + height, view.Left + 36, "Margin", ConsoleColor.DarkBlue, ConsoleColor.Yellow, 12);
                m_UI.Center(view.Top + height, view.Left + 48, $"Chance(%) ", ConsoleColor.DarkBlue, ConsoleColor.Yellow, 12);
                m_UI.Right(view.Top + height, view.Left + 60, " Races Won", ConsoleColor.DarkBlue, ConsoleColor.Yellow, 14);
                m_UI.Center(view.Top + height++, view.Left + 74, "Races Won(%)", ConsoleColor.DarkBlue, ConsoleColor.Yellow, 14);

                var topWinners = m_HorseRaceService.TopWinners;

                foreach (var runner in m_HorseRaceService.RaceRunners)
                {
                    string mark = topWinners.Contains(runner.Horse) ? "*" : " ";
                    m_UI.Write(view.Top + height, view.Left, mark);
                    m_UI.Write(view.Top + height, view.Left + 1, $"{runner}");
                    m_UI.Right(view.Top + height, view.Left + 24, $"{runner.OddsPrice}", ConsoleColor.White, ConsoleColor.DarkBlue, 8);
                    m_UI.Write(view.Top + height, view.Left + 37, $"{Math.Round(runner.OddsPrice.Margin, 2, MidpointRounding.ToEven),8}");
                    m_UI.Write(view.Top + height, view.Left + 47, $"{runner.ChanceOfWinning(m_HorseRaceService.RaceMargin),8}");
                    m_UI.Right(view.Top + height, view.Left + 62, $"{runner.Horse.RacesWon:N0}", ConsoleColor.White, ConsoleColor.DarkBlue, 12);

                    if (m_HorseRaceService.TotalRaceRuns > 0)
                    {
                        {
                            var p = decimal.Divide(runner.Horse.RacesWon, m_HorseRaceService.TotalRaceRuns);
                            m_UI.Right(view.Top + height, view.Left + 74, $"{Math.Round(p * 100, 2, MidpointRounding.ToEven):N2}", ConsoleColor.White, ConsoleColor.DarkBlue, 10);
                        }
                    }

                    height++;
                }
            }
        }
        /// <summary>
        /// Display the menu options
        /// </summary>
        /// <param name="top">Top row</param>
        /// <param name="left">Left Column</param>
        /// <param name="top">Offset from top</param>
        /// <returns></returns>
        private void ShowMenu((int Top, int Left, int Height, int Width) view)
        {
            int height = 0;

            m_UI.Center(view.Top + height++, view.Left, "Options ", ConsoleColor.Yellow, ConsoleColor.Blue, view.Width);

            foreach (var cmd in MainMenu)
            {
                var f = cmd.Value.Enabled() ? ConsoleColor.White : ConsoleColor.DarkGray;
                m_UI.Write(view.Top + height++, view.Left + 4, $"{cmd.Key} -> {cmd.Value.Prompt}", f);
            }
            height += 1;

            m_UI.Write(view.Top + height++, view.Left, "Select an option or press Ctrl-C to abort.");
        }
        /// <summary>
        /// Display any available messages.
        /// </summary>
        private void ShowMessages()
        {
            var count = m_Messages.Count;

            foreach (var m in m_Messages)
                m_UI.Write((View.Height) - count--, View.Left, $"==> {m}", ConsoleColor.White, ConsoleColor.Red);

            m_Messages.Clear();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private void ProcessKey(ConsoleKeyInfo k)
        {
            if (MainMenu.Keys.Contains(k.Key))
            {
                try
                {
                    if (MainMenu[k.Key].Enabled() == true)
                    {
                        MainMenu[k.Key].Action();
                    }
                    else
                        m_Messages.Add($"This option is currently disabled.");
                }
                catch (InvalidOperationException ex)
                {
                    m_Logger.LogError(ex, "ProcessKey", null);
                    m_Messages.Add($"{ex.Message}");
                    m_Messages.Add($"Operation aborted.");
                }
            }
        }
        private void ClearConsole()
        {
            m_UI.SetForegroundColor(ConsoleColor.White);
            m_UI.SetBackgroundColor(ConsoleColor.DarkBlue);

            m_UI.Clear();
        }
        private void Initialize()
        {
            m_UI.SetWindowSize(View.Height, View.Width);
            m_UI.SetBufferSize(View.Height, View.Width);

            MainMenu = new Dictionary<ConsoleKey, Command>()
            {
                {
                    ConsoleKey.A,
                    new Command("Add a new runner", () =>
                    {
                        var name = string.Empty;
                        string oddsText = string.Empty;
                        var isCompliant = true;
                        var isEmpty= true;

                        m_Logger.LogInformation($"Executing command: Add a new runner.");


                        do
                        {
                            ClearConsole();
                            ShowTitle(AddRunnerView, "Add Runner");

                            m_UI.Write(AddRunnerView.Top + 2, AddRunnerView.Left, "In order to add a runner, enter a name compliant to British Horseracing Authority rules", ConsoleColor.Cyan);
                            m_UI.Write(AddRunnerView.Top + 3, AddRunnerView.Left, "and then enter the odds price in the format x/y, where x and y are natural numbers.", ConsoleColor.Cyan);
                            m_UI.Write(AddRunnerView.Top + 5, AddRunnerView.Left, "Leave any field empty to abort the addition.", ConsoleColor.Cyan);
                            ShowMessages();

                            m_UI.CursorVisibility = true;

                            m_UI.Write(AddRunnerView.Top + 8, AddRunnerView.Left, "Enter the runner name (Leave empty to abort):", ConsoleColor.Yellow);

                            m_UI.SetCursorPosition(AddRunnerView.Top + 8, AddRunnerView.Left + 46);
                            name = m_InputReader.ReadLine();

                            isCompliant = Domain.Horse.IsCompliant(name);
                            isEmpty = string.IsNullOrWhiteSpace(name);

                            if (!isCompliant && !isEmpty)
                            {
                                m_Messages.Add($"Runner name [{name}] is not compliant.");
                            }

                        } while (!isCompliant && !isEmpty);

                        if (isCompliant)
                        {
                            do
                            {
                                ShowMessages();

                                m_UI.Write(AddRunnerView.Top+10, AddRunnerView.Left, $"Enter the odds price (Leave empty to abort):", ConsoleColor.Yellow);

                                m_UI.SetCursorPosition(AddRunnerView.Top + 10, AddRunnerView.Left + 46);
                                oddsText = m_InputReader.ReadLine();

                                isEmpty = string.IsNullOrWhiteSpace(oddsText);
                                isCompliant = OddsPrice.IsCompliant(oddsText);

                                if (!isCompliant && !isEmpty)
                                {
                                    m_Messages.Add($"Runner odds price [{oddsText}] is not in the format x/y, where x and y are natural numbers.");
                                }
                            } while (!isCompliant && !isEmpty);

                                if (isCompliant)
                                    m_HorseRaceService.AddRaceRunner(name, oddsText);
                        }
                        else
                        {
                            if (!isEmpty)
                                m_Messages.Add("Runner name is not compliant to British Horseracing Authority rules...");
                        }
                    }, () => m_HorseRaceService.IsRaceFull == false)
                },
                {
                    ConsoleKey.D,
                    new Command("Add demo runners", () =>
                    {
                        m_Logger.LogInformation($"Executing command: Add demo runners.");
                        m_HorseRaceService.AddDemoRunners();

                    }, () => m_HorseRaceService.IsRaceEmpty == true)
                },
                {
                    ConsoleKey.C,
                    new Command("Reset race state", () =>
                    {
                        m_Logger.LogInformation($"Executing command: Reset race state.");
                        m_HorseRaceService.ClearRaceState();

                    }, () => m_HorseRaceService.IsRaceEmpty == false && m_HorseRaceService.IsRaceRunning==false)
                },
                 {
                    ConsoleKey.R,
                    new Command("Run the simulation once", () =>
                    {
                        m_Logger.LogInformation($"Executing command: Run the simulation once.");
                        try
                        {
                            m_HorseRaceService.StartRace(1, (it) => { }, CancellationToken.None);
                        }
                        catch (InvalidOperationException ex)
                        {
                            m_Logger.LogError(ex, "Running the simulation", null);
                            m_Messages.Add($"{ex.Message}");
                        }
                    }, () => m_HorseRaceService.IsRaceReady == true)
                },
                {
                    ConsoleKey.S,
                    new Command("Stop the running simulation", () =>
                    {
                        m_Logger.LogInformation($"Executing command: Stop the running simulation.");
                        m_TokenSource.Cancel();
                        m_TokenSource.Dispose();

                    }, () => m_HorseRaceService.IsRaceRunning == true)
                },
                {
                    ConsoleKey.B,
                    new Command("Start the simulation multiple times", () =>
                    {
                        m_Logger.LogInformation($"Executing command: Start the simulation multiple times");

                        ClearConsole();
                        ShowMessages();

                        m_UI.Write(10, 4, $"Enter the number of times to run (default: 1)");
                        m_UI.SetCursorPosition(10, 52);

                        string repetitionsText = m_InputReader.ReadLine();

                        if (!int.TryParse(repetitionsText, out int times))
                            times = 1;

                        if (times > 0)
                        {
                            m_TokenSource = new CancellationTokenSource();
                            CancellationToken token = m_TokenSource.Token;

                            m_Logger.LogInformation($"Starting {times} iterations of the simulation");

                            Task.Factory.StartNew(() =>
                            {
                                try
                                {
                                    m_HorseRaceService.StartRace(times, (it) =>
                                    {
                                        ShowRace();
                                        ShowMenu(MainMenuView);

                                    }, token);
                                }
                                catch (InvalidOperationException ex)
                                {
                                    m_Logger.LogError(ex, "Simulation multiple times", null);
                                    m_Messages.Add($"{ex.Message}");
                                }
                            }, token);
                        }
                    }, () => m_HorseRaceService.IsRaceReady == true)
                }
        };
        }
        public void Dispose()
        {
            m_TokenSource?.Dispose();
            m_Timer?.Dispose();
        }

        #endregion

        #region constructors
        public HorseRaceUIService(ILogger<HorseRaceUIService> logger, IHorseRaceService horseRaceService, IConsoleWriter ui, IConsoleReader reader)
        {
            m_Logger = logger ?? throw new InvalidOperationException("Cannot utilize Logging facility.");
            m_HorseRaceService = horseRaceService ?? throw new InvalidOperationException("Cannot obtain Horse Race Event.");
            m_UI = ui ?? throw new InvalidOperationException("No UI surface provided.");
            m_InputReader = reader ?? throw new InvalidOperationException("No InptutReader provided.");

            Initialize();

            // Kick start
            m_Timer = new Timer(this.Execute, null, 10, Timeout.Infinite);
        }

        #endregion

        #region members

        private Dictionary<ConsoleKey, Command> MainMenu;

        private readonly IHorseRaceService m_HorseRaceService;
        private readonly ILogger<HorseRaceUIService> m_Logger;

        private readonly IConsoleWriter m_UI;
        private readonly IConsoleReader m_InputReader;

        private readonly Timer m_Timer;
        private CancellationTokenSource m_TokenSource = null;

        private readonly List<string> m_Messages = new List<string>();

        #endregion
    }
}
