namespace EasyConsole
{
    public class ActionMenuWithDefault : Menu
    {
        private readonly string _prompt;
        private const int DefaultOptionIndex = 0;

        private Option DefaultOption => Options[DefaultOptionIndex];

        public ActionMenuWithDefault(string prompt, string defaultOption, Func<Task> @default) : this(prompt, new Option(defaultOption, @default))
        {
        }

        public ActionMenuWithDefault(string prompt, Option defaultOption)
        {
            _prompt = prompt.Trim();
            Add(defaultOption);
            if (Options.Count != 1)
            {
                throw new InvalidOperationException($"Expected {nameof(Options)} to have only one option after adding the default");
            }
        }

        protected override Option DisplayPrompt()
        {
            var choice = Input.ReadIntDoNotAppendDefaultToPrompt($"{_prompt}  [{DefaultOptionIndex + 1}. {DefaultOption.Name}]", GetOptionRange(), DefaultOptionIndex + 1);
            return GetOption(choice);
        }
    }
}
