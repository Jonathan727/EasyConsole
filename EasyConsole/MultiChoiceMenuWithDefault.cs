namespace EasyConsole
{
    public abstract class MultiChoiceMenuWithDefault<TValue, TOption, TReturn> : MultiChoiceMenuBase<TValue, TOption, TReturn> where TOption : ValueOption<TValue>
    {
        private readonly string _prompt;
        private const int DefaultOptionNumber = 1;
        private TOption DefaultOption => GetOption(DefaultOptionNumber);

        protected MultiChoiceMenuWithDefault(string prompt, TOption defaultOption)
        {
            _prompt = prompt.Trim();
            Add(defaultOption);
            if (Options.Count != 1)
            {
                throw new InvalidOperationException($"Expected {nameof(Options)} to have only one option after adding the default");
            }
        }

        protected override IEnumerable<TOption> DisplayPrompt()
        {
            Output.WriteLine(_prompt);
            var choices = Input.ReadMultiChoiceIntDoNotAppendDefaultToPrompt($"Choose options (comma delimited)  [{DefaultOptionNumber}. {DefaultOption.Name}]", GetOptionRange(), DefaultOptionNumber);
            return choices.Select(GetOption);
        }
    }
}
