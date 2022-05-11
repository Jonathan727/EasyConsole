namespace EasyConsole
{
    public abstract class SingleChoiceMenuBase<TValue, TOption, TReturn> : MenuBase<TValue, TOption, TOption, TReturn> where TOption : ValueOption<TValue>
    {
        protected override TOption DisplayPrompt()
        {
            var choice = HasDefaultOption
                ? Input.ReadIntDoNotAppendDefaultToPrompt(PromptText, GetOptionRange(), DefaultOptionNumber)
                : Input.ReadInt(PromptText, GetOptionRange());
            return GetOption(choice);
        }
    }
}
