namespace EasyConsole.Menu
{
    public abstract class MultiChoiceMenuBase<TValue, TOption, TReturn> : MenuBase<TValue, TOption, IEnumerable<TOption>, TReturn> where TOption : ValueOption<TValue>
    {
        protected override IEnumerable<TOption> DisplayPrompt()
        {
            var choices = Input.ReadMultiChoiceInt("Choose options (comma delimited)", GetOptionRange());
            return choices.Select(GetOption);
        }
    }
}
