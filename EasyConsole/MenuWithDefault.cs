namespace EasyConsole
{
    public class MenuWithDefault<T> : ValueMenu<T>
    {
        private readonly string _prompt;
        private const int DefaultOptionIndex = 0;

        private ValueOption<T> DefaultOption => Options[DefaultOptionIndex];

        public MenuWithDefault(string prompt, string defaultOption, T @default) : this(prompt, new ValueOption<T>(defaultOption, @default))
        {
        }

        public MenuWithDefault(string prompt, ValueOption<T> defaultOption)
        {
            _prompt = prompt.Trim();
            Add(defaultOption);
            if (Options.Count != 1)
            {
                throw new InvalidOperationException($"Expected {nameof(Options)} to have only one option after adding the default");
            }
        }

        protected override ValueOption<T> DisplayPrompt()
        {
            var choice = Input.ReadIntDoNotAppendDefaultToPrompt($"{_prompt}  [{DefaultOptionIndex + 1}. {DefaultOption.Name}]:", min: 1, max: Options.Count, DefaultOptionIndex + 1);
            var chosenOption = Options[choice - 1];
            return chosenOption;
        }
    }
}
