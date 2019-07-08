using System.Collections.Generic;
using System.Linq;

namespace Amdocs.HorseRacing.Domain
{
    /// <summary>
    /// ValueObjects are lighweight, immutable and have structural equality.
    /// This implementation is based on Microsoft's reference implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueObject
    {
        #region methods
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            ValueObject other = (ValueObject)obj;

            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }

                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }
        public static bool operator ==(ValueObject lhs, ValueObject rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null))
                return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;

            return lhs.Equals(rhs);
        }
        public static bool operator !=(ValueObject lhs, ValueObject rhs)
        {
            return !(lhs == rhs);
        }
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
        protected static bool EqualOperator(ValueObject lhs, ValueObject rhs)
        {
            if (ReferenceEquals(lhs, null) ^ ReferenceEquals(rhs, null))
            {
                return false;
            }
            return ReferenceEquals(lhs, null) || lhs.Equals(rhs);
        }
        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }
        protected abstract IEnumerable<object> GetAtomicValues();
        
        #endregion 
    }
}
