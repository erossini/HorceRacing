using System;

namespace Amdocs.HorseRacing.Console.UI
{
    internal interface IConsoleWriter
    {
        #region properties
        bool CursorVisibility { get; set; }

        #endregion

        #region methods
        void Clear();
        void Center(int top, int left, string text, ConsoleColor f, ConsoleColor b, int length);
        void Left(int top, int left, string text, ConsoleColor f, ConsoleColor b, int length);
        void Right(int top, int left, string text, ConsoleColor f, ConsoleColor b, int length);
        (int x, int y) GetCursorPosition();
        void SetBackgroundColor(ConsoleColor color);
        void SetCursorPosition(int top, int left);
        void SetForegroundColor(ConsoleColor color);
        void SetBufferSize(int height, int width);
        void SetWindowSize(int height, int width);
        (int Height, int Width) WindowSize();
        void Write(int top, int left, string text, ConsoleColor f, ConsoleColor b);
        void Write(int top, int left, string text);
        void Write(string text);
        void Write(int top, int left, string text, ConsoleColor f);

        #endregion
    }
}
