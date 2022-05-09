namespace EasyConsole
{
    public abstract class SingleChoiceMenuBase<TValue, TOption, TReturn> : MenuBase<TValue, TOption, TOption, TReturn> where TOption : ValueOption<TValue>
    {
        protected override TOption DisplayPrompt()
        {
            var choice = Input.ReadInt("Choose an option:", min: 1, max: Options.Count);
            var chosenOption = Options[choice - 1];
            return chosenOption;
        }
    }
}
