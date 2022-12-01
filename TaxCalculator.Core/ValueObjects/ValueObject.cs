namespace TaxCalculator.Core.ValueObjects
{
    public abstract class ValueObject<T, V> where T : ValueObject<T, V>
    {
        private int? _cachedHashCode;

        protected ValueObject(V value)
        {
            Value = value;
        }

        public V Value { get; private set; }

        public override bool Equals(object obj)
        {
            T val = obj as T;
            if (val == null)
            {
                return false;
            }

            return Value.Equals(val.Value);
        }

        public override int GetHashCode()
        {
            if (!_cachedHashCode.HasValue)
            {
                _cachedHashCode = HashCode.Combine(Value, 7532) ;
            }

            return _cachedHashCode.Value;
        }

        public static bool operator ==(ValueObject<T, V> a, ValueObject<T, V> b)
        {
            if ((object)a == null && (object)b == null)
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T, V> a, ValueObject<T, V> b)
        {
            return !(a == b);
        }
    }
}
