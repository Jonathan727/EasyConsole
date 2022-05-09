namespace EasyConsole
{
    /// <summary>
    /// Menu that returns a <see cref="ValueOption{T}.Value"/> from a selected option
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueMenu<T> : SingleChoiceMenuBase<T, ValueOption<T>, T>
    {
        protected override T OnOptionChosen(ValueOption<T> option)
        {
            return option.Value;
        }

        public ValueMenu<T> Add(string name, T value)
        {
            Add(new ValueOption<T>(name, value));
            return this;
        }
    }
}
