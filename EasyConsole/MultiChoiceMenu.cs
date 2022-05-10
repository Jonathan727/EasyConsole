namespace EasyConsole
{
    /// <summary>
    /// Similar to <see cref="ValueMenu{T}"/>, but allows multiple choices
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiChoiceMenu<T> : MultiChoiceMenuBase<T, ValueOption<T>, IReadOnlyCollection<T>>
    {
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
}