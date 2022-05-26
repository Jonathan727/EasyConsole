namespace EasyConsole
{

    public class MultiValueOption<T> : ValueOption<ICollection<T>>
    {
        public MultiValueOption(string name, IEnumerable<T> values) : base(name, values.ToArray())
        {
        }

        public MultiValueOption(string name, params T[] values) : base(name, values)
        {
            if (values.Length == 0)
            {
                throw new ArgumentException("At least one value is required", nameof(values));
            }
        }

        public new bool IsValueNull()
        {
            return base.IsValueNull() && Value.Any(x => x is null);
        }

        public bool Equals(MultiValueOption<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Value.SequenceEqual(other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MultiValueOption<T>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }

        public static bool operator ==(MultiValueOption<T>? left, MultiValueOption<T>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MultiValueOption<T>? left, MultiValueOption<T>? right)
        {
            return !Equals(left, right);
        }

        public override string ToString() => Name;
    }
}
