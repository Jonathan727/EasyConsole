using EasyConsole;

namespace Demo.Pages
{
    class Page1Ai : Page
    {
        public Page1Ai(Program program) : base("Page 1Ai", program)
        {
        }

        public override async Task Display()
        {
            await base.Display();

            Output.WriteLine("Hello from Page 1Ai");

            Input.ReadString("Press [Enter] to navigate home");
            await Program.NavigateHome();
        }
    }
}
