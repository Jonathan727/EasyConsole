namespace EasyConsole
{
    /// <summary>
    /// Menu that returns a <see cref="ValueOption{T}.Value"/> from a selected selection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueMenu<T> : SingleChoiceMenuBase<T, ValueOption<T>, T>
    {
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
