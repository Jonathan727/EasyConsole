﻿using EasyConsole;

namespace Demo.Pages
{
    class Page1B : Page
    {
        public Page1B(Program program) : base("Page 1B", program)
        {
        }

        public override async Task Display()
        {
            await base.Display();

            Output.WriteLine("Hello from Page 1B");

            Input.ReadString("Press [Enter] to navigate home");
            await Program.NavigateHome();
        }
    }
}
