using EasyConsole;
using System;

namespace Demo.Pages
{
    class InputPage : Page
    {
        private ValueOption<InputDemo>[] InputDemoOptions = new[]
        {
            new ValueOption<InputDemo>($"{typeof(Input)}.{nameof(Input.ReadOption)}", InputDemo.ReadOption),
        };

        public InputPage(Program program)
            : base("Input", program)
        {
        }

        public override async Task Display()
        {
            await base.Display();
            while (true) //TODO: provide a way to navigate home
            {
                var demo = await Input.ReadEnum<InputDemo>("Select a demo");
                Output.WriteLine(ConsoleColor.Green, "You selected {0}", demo);

                switch (demo)
                {
                    case InputDemo.ReadOption:
                    {
                        var stringOptions = new[]
                        {
                            "Alpha",
                            "Bravo",
                            "Delta",
                            "Charlie",
                        };
                        var input = Input.ReadOption(stringOptions);
                        Output.WriteLine(ConsoleColor.Green, "You selected {0}", input);
                        break;
                    }
                    case InputDemo.ReadEnum:
                    {
                        var input = await Input.ReadEnum<Fruit>("Select a fruit");
                        Output.WriteLine(ConsoleColor.Green, "You selected {0}", input);
                        break;
                    }
                    case InputDemo.MultiChoiceEnum:
                    {
                        var input = Input.ReadMultiChoiceEnum<Fruit>("Select multiple fruits");
                        Output.WriteLine(ConsoleColor.Green, "You selected {0}", string.Join(", ", input));
                        break;
                    }
                    case InputDemo.MultiChoiceInt:
                    {
                        const int min = -10000;
                        const int max = 10000;
                        var choices = Input.ReadMultiChoiceInt($"Select numbers between {min:N0} and {max:N0} (comma delimited)", min, max);

                        Output.WriteLine(ConsoleColor.Green, $"You selected {string.Join("; ", choices.Select(x => $"{x:n0}"))}");
                        break;
                    }
                    default:
                    {
                        Output.WriteLine(ConsoleColor.Red, $"Demo for {Enum.GetName(demo)} not implemented");
                        break;
                    }
                }
            }

            Input.ReadString("Press [Enter] to navigate home");
            await Program.NavigateHome();
        }
    }

    enum Fruit
    {
        Apple,
        Banana,
        Coconut,
    }

    enum InputDemo
    {
        ReadOption,
        ReadEnum,
        MultiChoiceEnum,
        MultiChoiceInt,
    }
}