namespace EasyConsole
{
    public class Menu
    {
        private IList<Option> Options { get; }

        public Menu()
        {
            Options = new List<Option>();
        }

        public async Task Display()
        {
            for (var i = 0; i < Options.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
            }

            var choice = Input.ReadInt("Choose an option:", min: 1, max: Options.Count);

            await Options[choice - 1].Callback();
        }

        public Menu Add(string option, Func<Task> callback)
        {
            return Add(new Option(option, callback));
        }

        public Menu Add(Option option)
        {
            Options.Add(option);
            return this;
        }

        public bool Contains(string option)
        {
            return Options.FirstOrDefault((op) => op.Name.Equals(option)) != null;
        }
    }
}