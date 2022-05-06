namespace EasyConsole
{
    /// <summary>
    /// Similar to <see cref="ValueMenu{T}"/>, but allows multiple choices
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiChoiceMenu<T> : MenuBase<T, ValueOption<T>, IReadOnlyCollection<T>>
    {
        public override IReadOnlyCollection<T> Display()
        {
            ValidateOptions(true);

            for (var i = 0; i < Options.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
            }

            var choices = Input.ReadMultiChoiceInt("Choose options (comma delimited): ", min: 1, max: Options.Count);
            return choices.Select(choice => Options[choice - 1].Value!).ToList();
        }

        public MultiChoiceMenu<T> Add(string name, T value)
        {
            Add(new ValueOption<T>(name, value));
            return this;
        }
    }
}