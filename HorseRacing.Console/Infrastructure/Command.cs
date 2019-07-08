using System;

namespace Amdocs.HorseRacing.Console.Infrastructure
{
    /// <summary>
    /// A menu option
    /// </summary>
    internal class Command
    {
        #region properties
        public string Prompt { get; private set; }
        public Action Action { get; private set; }
        public Func<bool> Enabled { get; private set; }

        #endregion

        #region constructors
        public Command(string prompt, Action action, Func<bool> isEnabled)
        {
            Prompt = prompt;
            Action = action;
            Enabled = isEnabled;
        }

        #endregion
    }
}
