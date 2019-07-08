using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Amdocs.HorseRacing.Domain
{
    /// <summary>
    /// Horse class is a wrapper around a horse name. It is a value type and it is immutable.
    /// The name is as per the British Horseracing Authority rules, a maximum of 18 characters long (A-Z only including spaces).
    /// </summary>
    public sealed class Horse : ValueObject
    {
        #region properties

        /// <summary>
        /// The name is as per the British Horseracing Authority rules, 
        /// a maximum of 18 characters long (A-Z only including spaces).
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets or sets the total number of races this horse has won.
        /// </summary>
        public long RacesWon { get; set; }

        #endregion

        #region constructors
        public Horse(string name)
        {
            if (!Horse.IsCompliant(name))
                throw new InvalidOperationException("The requested horse name is not compliant to the British Horseracing Authority rules.");

            // All Ok.
            Name = name;
            RacesWon = 0;
        }

        #endregion

        #region methods

        /// <summary>
        /// Checks if name is as per the British Horseracing Authority rules.
        /// </summary>
        /// <param name="name">Requested horse name</param>
        /// <returns>True if name is as per the British Horseracing Authority rules, false otherwise</returns>
        public static bool IsCompliant(string name) => !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, m_Pattern);
        public override string ToString() => $"{Name}";
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }

        #endregion

        #region members

        // A-Z only including spaces, up to a maximum of 18 characters.
        private const string m_Pattern = "^[A-Z ]{1,18}$";

        #endregion
    }
}