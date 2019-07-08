using System;

namespace Amdocs.HorseRacing.Domain
{
    /// <summary>
    /// Entities have their own identity.
    /// This implementation is based on Microsoft's reference implementation.
    /// </summary>
    public abstract class Entity<T> where T : IComparable
    {
        #region properties
        public virtual T Identity { get; protected set; }

        #endregion

        #region methods

        /// <summary>
        /// Identity not yet established
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return Identity.CompareTo(default(T)) == 0;
        }
        public override bool Equals(object obj)
        {
            var other = obj as Entity<T>;

            if (other is null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            if (ReferenceEquals(this, other))
                return true;

            // Identity not yet established
            if (this.IsTransient() || other.IsTransient())
                return false;

            return Identity.Equals(other.Identity);
        }
        public static bool operator ==(Entity<T> lhs, Entity<T> rhs)
        {
            if (lhs is null && rhs is null)
                return true;

            if (lhs is null || rhs is null)
                return false;

            return lhs.Equals(rhs);
        }
        public static bool operator !=(Entity<T> lhs, Entity<T> rhs)
        {
            return !(lhs == rhs);
        }
        /// <summary>
        /// XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!m_RequestedHashCode.HasValue)
                    m_RequestedHashCode = (GetType().ToString().GetHashCode() + this.Identity.GetHashCode()) ^ 31;

                return m_RequestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        #endregion

        #region members

        private int? m_RequestedHashCode;

        #endregion
    }
}
