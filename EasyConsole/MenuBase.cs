namespace EasyConsole
{
    public abstract class MenuBase<TValue, TOption, TReturn> where TOption : ValueOption<TValue>
    {
        protected List<TOption> Options { get; } = new();

        public abstract TReturn Display();

        protected void ValidateOptions(bool checkForNull)
        {
            if (!Options.Any())
            {
                throw new InvalidOperationException($"No items in {nameof(Options)}.");
            }
            if (checkForNull && Options.Any(x => x.Value is null))
            {
                throw new InvalidOperationException($"{typeof(TOption)}.{nameof(ValueOption<TValue>.Value)} was null or is not set in one or more {nameof(Options)}");
            }
        }

        public MenuBase<TValue, TOption, TReturn> Add(TOption option)
        {
            Options.Add(option);
            return this;
        }

        public MenuBase<TValue, TOption, TReturn> AddRange(IEnumerable<TOption> options)
        {
            Options.AddRange(options);
            return this;
        }

        public bool Contains(string option)
        {
            return Options.FirstOrDefault((op) => op.Name.Equals(option)) != null;
        }
    }
}
