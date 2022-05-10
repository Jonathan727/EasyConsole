namespace EasyConsole.Types
{
    /// <summary>
    /// Inclusive int range
    /// </summary>
    public readonly struct IntRange : IEquatable<IntRange>
    {
        public int Min { get; }

        public int Max { get; }

        public IntRange(int val1, int val2)
        {
            if (val1 <= val2)
            {
                Min = val1;
                Max = val2;
            }
            else
            {
                Min = val2;
                Max = val1;
            }
        }

        public bool IsInside(int x)
        {
            return x >= Min && x <= Max;
        }

        public bool IsOutside(int x)
        {
            return !IsInside(x);
        }

        public bool IsInside(IntRange range)
        {
            return IsInside(range.Min) && IsInside(range.Max);
        }

        public bool IsOverlapping(IntRange range)
        {
            return IsInside(range.Min) || IsInside(range.Max) ||
                   range.IsInside(Min) || range.IsInside(Max);
        }

        /// <summary>
        /// Equality operator - checks if two ranges have equal min/max values.
        /// </summary>
        public static bool operator ==(IntRange range1, IntRange range2)
        {
            return range1.Min == range2.Min && range1.Max == range2.Max;
        }

        /// <summary>
        /// Inequality operator - checks if two ranges have different min/max values.
        /// </summary>
        public static bool operator !=(IntRange range1, IntRange range2)
        {
            return range1.Min != range2.Min || range1.Max != range2.Max;

        }

        public bool Equals(IntRange other)
        {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            return obj is IntRange range && this == range;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Min.GetHashCode() * 397) ^ Max.GetHashCode();
            }
        }

        public override string ToString() => $"({Min}, {Max})";
    }
}