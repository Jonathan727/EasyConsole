namespace EasyConsole
{
    public abstract class SingleChoiceMenuWithDefault<TValue, TOption, TReturn> : SingleChoiceMenuBase<TValue, TOption, TReturn> where TOption : ValueOption<TValue>
    {
        private readonly string _prompt;
        private const int DefaultOptionNumber = 1;
        private TOption DefaultOption => GetOption(DefaultOptionNumber);

        protected SingleChoiceMenuWithDefault(string prompt, TOption defaultOption)
        {
            _prompt = prompt.Trim();
            Add(defaultOption);
            if (Options.Count != 1)
            {
                throw new InvalidOperationException($"Expected {nameof(Options)} to have only one option after adding the default");
            }
        }

        protected override TOption DisplayPrompt()
        {
            var choice = Input.ReadIntDoNotAppendDefaultToPrompt($"{_prompt}  [{DefaultOptionNumber}. {DefaultOption.Name}]", GetOptionRange(), DefaultOptionNumber);
            return GetOption(choice);
        }
    }
}
