using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Amdocs.HorseRacing.Domain
{
    /// <summary>
    /// OddsPrice class is a wrapper around the Odds price. It is a value type and it is immutable.
    /// Be able to be given a valid UK fractional odds price. 
    /// This is in the string format of “x/y” where x and y are integer numbers greater than 0 e.g 2/1, 10/1, 5/2
    /// </summary>
    public sealed class OddsPrice : ValueObject
    {
        #region properties

        /// To calculate the margin firstly convert each fractional odds into it’s decimal equivalent
        /// which is numerator divided by denominator and then add one. Then Divide 100 by decimal equivalent.
        public decimal Margin => 100m / ((Decimal.Divide(Numerator, Denominator)) + 1);
        public int Numerator { get; }
        public int Denominator { get; }

        #endregion

        #region constructors
        public OddsPrice(string oddsText)
        {
            if (string.IsNullOrWhiteSpace(oddsText))
                throw new InvalidOperationException("Odds price is null or empty.");

            Match match = new Regex(m_Pattern).Match(oddsText);

            if (!match.Success)
                throw new InvalidOperationException("The odds price is not in the format x/y, where x and y are natural numbers.");

            try
            {
                int numerator = Convert.ToInt32(match.Groups[1].Value);
                int denominator = Convert.ToInt32(match.Groups[2].Value);

                //  greater or equal to 0 (because 1 is acceptable according to the spec examples)
                if (numerator < 1 || denominator < 1)
                    throw new InvalidOperationException($"Both numerator and denominator should be greater than 0.");

                // All Ok.
                Numerator = numerator;
                Denominator = denominator;
                m_Text = oddsText;
            }
            catch (OverflowException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        #endregion

        #region methods

        public static bool IsCompliant(string oddsText)
        {
            if (string.IsNullOrEmpty(oddsText))
                return false;

            return new Regex(m_Pattern).IsMatch(oddsText);
        }
        public override string ToString() => m_Text;
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Numerator;
            yield return Denominator;
        }

        #endregion

        #region members

        private readonly string m_Text = string.Empty;

        // No spaces allowed, as per example in document
        private const string m_Pattern = @"^(\d+)\/(\d+)$";

        #endregion
    }
}