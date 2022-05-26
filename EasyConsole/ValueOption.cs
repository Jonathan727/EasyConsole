namespace EasyConsole
{

    public class ValueOption<T> : IEquatable<ValueOption<T>>, IOption
    {
        public string Name { get; }

        public T Value { get; }

        public ValueOption(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public bool IsValueNull()
        {
            return Value == null;
        }

        public bool Equals(ValueOption<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValueOption<T>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }

        public static bool operator ==(ValueOption<T>? left, ValueOption<T>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueOption<T>? left, ValueOption<T>? right)
        {
            return !Equals(left, right);
        }

        public override string ToString() => Name;
    }
}
