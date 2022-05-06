namespace EasyConsole
{
    /// <summary>
    /// Menu that returns a <see cref="ValueOption{T}.Value"/> from a selected option
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueMenu<T> : MenuBase<T, ValueOption<T>, T>
    {
        public override T Display()
        {
            ValidateOptions(true);

            for (var i = 0; i < Options.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
            }

            var choice = Input.ReadInt("Choose an option:", min: 1, max: Options.Count);
            return Options[choice - 1].Value;
        }

        public ValueMenu<T> Add(string name, T value)
        {
            Add(new ValueOption<T>(name, value));
            return this;
        }
    }
}
