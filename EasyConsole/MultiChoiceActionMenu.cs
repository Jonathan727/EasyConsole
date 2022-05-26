namespace EasyConsole
{
    /// <summary>
    /// Similar to <see cref="Menu"/>, but allows multiple choices
    /// </summary>
    public class MultiChoiceActionMenu : MultiChoiceMenuBase<Func<Task>, Option, Task>
    {
        public MultiChoiceActionMenu()
        {
        }

        public MultiChoiceActionMenu(string? prompt, string defaultOption, Func<Task> defaultValue) : this(prompt, new Option(defaultOption, defaultValue))
        {
        }

        public MultiChoiceActionMenu(string? prompt, Option defaultOption) : base(prompt)
        {
            DefaultOption = defaultOption;
        }

        protected override async Task OnUserAnsweredPrompt(IEnumerable<Option> selection)
        {
            await Task.WhenAll(selection.Select(x => x.Value()));
        }

        public void Add(string name, Func<Task> callback)
        {
            Add(new Option(name, callback));
        }
    }
}