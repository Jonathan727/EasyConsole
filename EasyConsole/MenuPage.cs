namespace EasyConsole
{
    public abstract class MenuPage : Page
    {
        protected Menu Menu { get; set; }

        protected MenuPage(string title, Program program, params Option[] options) : base(title, program)
        {
            Menu = new Menu();
            Menu.AddRange(options);
        }

        public override async Task Display()
        {
            await base.Display();

            const string goBackOption = "Go back";
            if (Program.NavigationEnabled && !Menu.Contains(goBackOption))
            {
                Menu.Add(goBackOption, () => Program.NavigateBack());
            }

            await Menu.Display();
        }
    }
}
