namespace EasyConsole
{
    public class ActionMenuWithDefault : SingleChoiceMenuWithDefault<Func<Task>, Option, Task>
    {
        public ActionMenuWithDefault(string prompt, string defaultOption, Func<Task> @default) : this(prompt, new Option(defaultOption, @default))
        {
        }

        public ActionMenuWithDefault(string prompt, Option defaultOption) : base(prompt, defaultOption)
        {
        }

        protected override async Task OnUserAnsweredPrompt(Option selection)
        {
            await selection.Value();
        }

        public ActionMenuWithDefault Add(string name, Func<Task> callback)
        {
            Add(new Option(name, callback));
            return this;
        }
    }
}
