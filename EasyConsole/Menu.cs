namespace EasyConsole
{
    /// <summary>
    /// Menu where the options perform some asynchronous task
    /// </summary>
    public class Menu : SingleChoiceMenuBase<Func<Task>, Option, Task>
    {
        public Menu()
        {
        }

        public Menu(string prompt, string defaultOption, Func<Task> defaultValue) : this(prompt, new Option(defaultOption, defaultValue))
        {
        }

        public Menu(string prompt, Option defaultOption)
        {
            PromptText = prompt;
            DefaultOption = defaultOption;
        }

        protected override async Task OnUserAnsweredPrompt(Option selection)
        {
            await selection.Value();
        }

        public Menu Add(string name, Func<Task> callback)
        {
            Add(new Option(name, callback));
            return this;
        }
    }
}