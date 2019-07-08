using Amdocs.HorseRacing.Console.Infrastructure;
using System;
using System.IO;

namespace Amdocs.HorseRacing.Console.UI
{
    internal class SimpleConsoleWriter : IConsoleWriter
    {
        #region properties
        public bool CursorVisibility
        {
            get { return System.Console.CursorVisible; }
            set { System.Console.CursorVisible = value; }
        }

        #endregion

        #region methods
        public void Clear() => System.Console.Clear();
        public (int x, int y) GetCursorPosition() => (System.Console.CursorLeft, System.Console.CursorTop);
        public void SetCursorPosition(int top, int left)
        {
            System.Console.CursorLeft = left;
            System.Console.CursorTop = top;
        }
        public void SetBackgroundColor(ConsoleColor color) => System.Console.BackgroundColor = color;
        public void SetForegroundColor(ConsoleColor color) => System.Console.ForegroundColor = color;
        public void SetBufferSize(int height, int width) => System.Console.SetBufferSize(width, height);
        public void SetWindowSize(int height, int width) => System.Console.SetWindowSize(width, height);
        public (int Height, int Width) WindowSize() => (System.Console.WindowHeight, System.Console.WindowWidth);
        public void Write(string text) => Write(System.Console.CursorLeft, System.Console.CursorLeft, text);
        public void Write(int top, int left, string text) => Write(top, left, text, System.Console.ForegroundColor, System.Console.BackgroundColor);
        public void Write(int top, int left, string text, ConsoleColor f) => Write(top, left, text, f, System.Console.BackgroundColor);
        public void Center(int top, int left, string text, ConsoleColor f, ConsoleColor b, int length)
        {
            string s = string.Empty;

            if (length > text.Length)
                s = new string(' ', (length - text.Length) >> 1);

            Write(top, left, s + text + s, f, b);
        }
        public void Left(int top, int left, string text, ConsoleColor f, ConsoleColor b, int length)
        {
            string s = string.Empty;

            if (length > text.Length)
                s = new string(' ', length - text.Length);

            Write(top, left, text + s, f, b);
        }
        public void Right(int top, int left, string text, ConsoleColor f, ConsoleColor b, int length)
        {
            string s = string.Empty;

            if (length > text.Length)
                s = new string(' ', length - text.Length);

            Write(top, left, s + text, f, b);
        }
        public void Write(int top, int left, string text, ConsoleColor f, ConsoleColor b)
        {
            lock (this)
            {
                var currentBackground = System.Console.BackgroundColor;
                var currentForeground = System.Console.ForegroundColor;

                var cursorLeft = System.Console.CursorLeft;
                var cursorTop = System.Console.CursorTop;

                System.Console.BackgroundColor = b;
                System.Console.ForegroundColor = f;

                System.Console.CursorLeft = Math.Min(System.Console.WindowWidth, left);
                System.Console.CursorTop = Math.Min(System.Console.WindowHeight, top);

                m_Writer.Write(text);

                System.Console.BackgroundColor = currentBackground;
                System.Console.ForegroundColor = currentForeground;

                System.Console.CursorLeft = cursorLeft;
                System.Console.CursorTop = cursorTop;
            }
        }

        #endregion

        #region constructors
        public SimpleConsoleWriter()
        {
            IntPtr handle = NativeCommands.GetConsoleWindow();
            IntPtr sysMenu = NativeCommands.GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                NativeCommands.DeleteMenu(sysMenu, NativeCommands.SC_MAXIMIZE, NativeCommands.MF_BYCOMMAND);
                NativeCommands.DeleteMenu(sysMenu, NativeCommands.SC_SIZE, NativeCommands.MF_BYCOMMAND);
            }

            // Initialize the cursor to invisible.
            CursorVisibility = false;
            SetCursorPosition(1, 1);

            SetForegroundColor(ConsoleColor.White);
            SetBackgroundColor(ConsoleColor.Blue);

            Clear();
        }
        #endregion

        #region members

        private readonly TextWriter m_Writer = System.Console.Out;

        #endregion
    }
}
