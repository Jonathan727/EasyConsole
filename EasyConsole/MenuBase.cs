namespace EasyConsole
{
    public abstract class MenuBase<TValue, TOption, TChoice, TReturn> where TOption : ValueOption<TValue>
    {
        protected List<TOption> Options { get; } = new();
        protected bool AllowNullOptionValues { get; }

        protected MenuBase(bool allowNullOptionValues = false)
        {
            AllowNullOptionValues = allowNullOptionValues;
        }

        public TReturn Display()
        {
            ValidateOptions(!AllowNullOptionValues);

            DisplayOptions();

            var chosenOption = DisplayPrompt();

            return OnOptionChosen(chosenOption);
        }

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

        protected void DisplayOptions()
        {
            for (var i = 0; i < Options.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
            }
        }

        protected abstract TChoice DisplayPrompt();

        protected abstract TReturn OnOptionChosen(TChoice option);

        public MenuBase<TValue, TOption, TChoice, TReturn> Add(TOption option)
        {
            Options.Add(option);
            return this;
        }

        public MenuBase<TValue, TOption, TChoice, TReturn> AddRange(IEnumerable<TOption> options)
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
