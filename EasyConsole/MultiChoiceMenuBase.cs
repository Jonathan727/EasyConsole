namespace EasyConsole
{
    public abstract class MultiChoiceMenuBase<TValue, TOption, TReturn> : MenuBase<TValue, TOption, IEnumerable<TOption>, TReturn> where TOption : ValueOption<TValue>
    {
        protected string? PrePrompt { get; }

        public MultiChoiceMenuBase(string? prompt = null)
        {
            PrePrompt = prompt;
            PromptText = "Choose options (comma delimited)";
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
