namespace EasyConsole
{
    public class MultiChoiceMenu<T>
    {
        private IList<ValueOption<T>> Options { get; } = new List<ValueOption<T>>();

        public IReadOnlyCollection<T> Display()
        {
            for (var i = 0; i < Options.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
            }

            if (!Options.Any())
            {
                throw new InvalidOperationException($"No items in {nameof(Options)}.");
            }

            var choices = Input.ReadMultiChoiceInt("Choose options (comma delimited):", min: 1, max: Options.Count);

            return choices.Select(choice => Options[choice - 1].Value).ToList();
        }

        public MultiChoiceMenu<T> Add(string name, T value)
        {
            return Add(new ValueOption<T>(name, value));
        }

        public MultiChoiceMenu<T> Add(ValueOption<T> option)
        {
            Options.Add(option);
            return this;
        }

        public bool Contains(string name)
        {
            return Options.FirstOrDefault((op) => op.Name.Equals(name)) != null;
        }
    }
}