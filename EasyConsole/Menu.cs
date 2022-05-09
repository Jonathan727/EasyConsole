namespace EasyConsole
{
    /// <summary>
    /// Menu where the options perform some asynchronous task
    /// </summary>
    public class Menu : SingleChoiceMenuBase<Func<Task>, Option, Task>
    {
        protected override async Task OnOptionChosen(Option option)
        {
            await option.Value();
        }

        public Menu Add(string name, Func<Task> callback)
        {
            Add(new Option(name, callback));
            return this;
        }
    }
}