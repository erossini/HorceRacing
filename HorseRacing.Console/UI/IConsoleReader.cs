using System;

namespace Amdocs.HorseRacing.Console.UI
{
    internal interface IConsoleReader
    {
        #region methods
        int Read();
        ConsoleKeyInfo ReadKey();
        ConsoleKeyInfo ReadKey(bool intercept);
        string ReadLine();

        #endregion
    }
}
