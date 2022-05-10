namespace EasyConsole
{

    public class ValueOption<T>
    {
        public string Name { get; }

        public T Value { get; }

        public ValueOption(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => Name;
    }
}
