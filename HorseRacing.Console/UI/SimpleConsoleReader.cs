using System;

namespace Amdocs.HorseRacing.Console.UI
{
    internal class SimpleConsoleReader : IConsoleReader
    {
        #region methods

        public int Read()
        {
            return System.Console.Read();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey();
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return System.Console.ReadKey(intercept);
        }

        public string ReadLine()
        {
            return System.Console.ReadLine();
        }

        #endregion
    }
}
