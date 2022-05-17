namespace EasyConsole
{
    /// <summary>
    /// Menu that returns a <see cref="ValueOption{T}.Value"/> from a selected selection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueMenu<T> : SingleChoiceMenuBase<T, ValueOption<T>, T>
    {
        public ValueMenu()
        {
        }

        public ValueMenu(string prompt, string defaultOption, T defaultValue) : this(prompt, new ValueOption<T>(defaultOption, defaultValue))
        {
        }

        public ValueMenu(string prompt, ValueOption<T> defaultOption)
        {
            PromptText = prompt;
            DefaultOption = defaultOption;
        }

        protected override T OnUserAnsweredPrompt(ValueOption<T> selection)
        {
            return selection.Value;
        }

        public ValueMenu<T> Add(string name, T value)
        {
            Add(new ValueOption<T>(name, value));
            return this;
        }
    }
}
