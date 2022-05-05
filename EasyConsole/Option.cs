namespace EasyConsole
{
    public class Option
    {
        public string Name { get; }

        public Func<Task> Callback { get; }

        public Option(string name, Func<Task> callback)
        {
            Name = name;
            Callback = callback;
        }

        public override string ToString() => Name;
    }
}