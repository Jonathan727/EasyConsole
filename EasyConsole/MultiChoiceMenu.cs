namespace EasyConsole
{
    /// <summary>
    /// Similar to <see cref="ValueMenu{T}"/>, but allows multiple choices
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiChoiceMenu<T> : MultiChoiceMenuBase<T, ValueOption<T>, IReadOnlyCollection<T>>
    {
        public MultiChoiceMenu()
        {
        }

        public MultiChoiceMenu(string? prompt, string defaultOption, T defaultValue) : this(prompt, new ValueOption<T>(defaultOption, defaultValue))
        {
        }

        public MultiChoiceMenu(string defaultOption, T defaultValue) : this(null, new ValueOption<T>(defaultOption, defaultValue))
        {
        }

        public MultiChoiceMenu(string? prompt, ValueOption<T> defaultOption) : base(prompt)
        {
            DefaultOption = defaultOption;
        }

        protected override IReadOnlyCollection<T> OnUserAnsweredPrompt(IEnumerable<ValueOption<T>> selection)
        {
            return selection.Select(x => x.Value).ToList();
        }

        public MultiChoiceMenu<T> Add(string name, T value)
        {
            Add(new ValueOption<T>(name, value));
            return this;
        }
    }

    /// <summary>
    /// Similar to <see cref="MultiChoiceMenu{T}"/>, but allows each option to refer to one or more <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDefault">Multiple <typeparamref name="T"/></typeparam>
    public class MultiChoiceMenu<T, TDefault> : MultiChoiceMenuBase<T, MultiValueOption<T>, IReadOnlyCollection<T>> where TDefault : IEnumerable<T>
    {
        public MultiChoiceMenu(string? prompt, string defaultOption, TDefault defaultValues) : this(prompt, new MultiValueOption<T>(defaultOption, defaultValues.ToArray()))
        {
        }

        public MultiChoiceMenu(string defaultOption, params T[] defaultValues) : this(null, new MultiValueOption<T>(defaultOption, defaultValues))
        {
        }

        public MultiChoiceMenu(string? prompt, MultiValueOption<T> defaultOption) : base(prompt)
        {
            DefaultOption = defaultOption;
        }

        protected override IReadOnlyCollection<T> OnUserAnsweredPrompt(IEnumerable<MultiValueOption<T>> selection)
        {
            return selection.SelectMany(x => x.Value).ToList();
        }

        public void Add(string name, T[] values)
        {
            Add(new MultiValueOption<T>(name, values));
        }
    }
}