namespace EasyConsole
{
    public abstract class MultiChoiceMenuBase<TValue, TOption, TReturn> : MenuBase<TValue, TOption, IEnumerable<TOption>, TReturn> where TOption : class, IOption
    {
        public new string PromptText
        {
            get => base.PromptText;
            init => PrePrompt = value;
        }

        protected string? PrePrompt { get; init; }

        public MultiChoiceMenuBase(string? prompt = null)
        {
            PrePrompt = prompt;
            base.PromptText = "Choose options (comma delimited)";
        }

        protected override IEnumerable<TOption> DisplayPrompt()
        {
            if (PrePrompt != null)
            {
                Output.WriteLine(PrePrompt);
            }
            var choices = HasDefaultOption
                ? Input.ReadMultiChoiceIntDoNotAppendDefaultToPrompt(PromptText, GetOptionRange(), DefaultOptionNumber)
                : Input.ReadMultiChoiceInt(PromptText, GetOptionRange());
            return choices.Select(GetOption);
        }
    }
}
