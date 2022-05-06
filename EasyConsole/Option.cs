namespace EasyConsole
{
    /// <summary>
    /// An option that performs some task
    /// </summary>
    /// <seealso cref="Menu"/>
    public class Option : ValueOption<Func<Task>>
    {
        public Option(string name, Func<Task> value) : base(name, value)
        {
        }
    }
}