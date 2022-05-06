using EasyConsole;
using System;

namespace Demo.Pages
{
    class InputPage : Page
    {
        public InputPage(Program program)
            : base("Input", program)
        {
        }

        public override async Task Display()
        {
            await base.Display();

            var input = await Input.ReadEnum<Fruit>("Select a fruit");
            Output.WriteLine(ConsoleColor.Green, "You selected {0}", input);

            const int min = -10000;
            const int max = 10000;
            var choices = Input.ReadMultiChoiceInt($"Select numbers between {min:N0} and {max:N0} (comma delimited)", min, max);

            Output.WriteLine(ConsoleColor.Green, $"You selected {string.Join("; ", choices.Select(x => $"{x:n0}"))}");

            Input.ReadString("Press [Enter] to navigate home");
            await Program.NavigateHome();
        }
    }

    enum Fruit
    {
        Apple,
        Banana,
        Coconut
    }
}
