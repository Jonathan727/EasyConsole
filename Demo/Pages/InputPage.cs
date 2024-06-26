﻿using EasyConsole;

namespace Demo.Pages
{
    class InputPage : Page
    {
        public string? FavoritePlace { get; set; } = "Home";

        public InputPage(Program program) : base("Input", program)
        {
        }

        public override async Task Display()
        {
            await base.Display();
            while (true)
            {
                var demo = Input.ReadEnum<InputDemo>("Select a demo");
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
                        var input = Input.ReadEnum<Fruit>("Select a fruit");
                        Output.WriteLine(ConsoleColor.Green, "You selected {0}", input);
                        break;
                    }
                    case InputDemo.ReadEnumWithDefault:
                    {
                        var input = Input.ReadEnum("Select a fruit", Fruit.Banana);
                        Output.WriteLine(ConsoleColor.Green, "You selected {0}", input);
                        break;
                    }
                    case InputDemo.MultiChoiceEnum:
                    {
                        var input = Input.ReadMultiChoiceEnum<Fruit>("Select multiple fruits");
                        Output.WriteLine(ConsoleColor.Green, "You selected {0}", string.Join(", ", input));
                        break;
                    }
                    case InputDemo.MultiChoiceEnumWithDefault:
                    {
                        var input = Input.ReadMultiChoiceEnum("Select multiple fruits", Fruit.Banana);
                        Output.WriteLine(ConsoleColor.Green, "You selected {0}", string.Join(", ", input));
                        break;
                    }
                    case InputDemo.MultiChoiceMenuEnumWithAllDefault:
                    {
                        var menu = new MultiChoiceMultiValueMenu<Fruit>("Select multiple fruits", "(All)", Enum.GetValues<Fruit>())
                        {
                            Options = GetEnumNamesAndValues<Fruit>().Select(x => new MultiValueOption<Fruit>(x.name, x.value)).ToList(),
                        };
                        var result = menu.Display();
                        Output.WriteLine(ConsoleColor.Green, "You selected {0}", string.Join(", ", result));
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
                    case InputDemo.String:
                    {
                        FavoritePlace = Input.ReadString("What is your favorite place?");
                        Output.WriteLine(ConsoleColor.Green, $"Your favorite place is '{FavoritePlace}'");
                        break;
                    }
                    case InputDemo.StringWithDefault:
                    {
                        FavoritePlace = Input.ReadStringWithDefault("What is your favorite place?", FavoritePlace);
                        Output.WriteLine(ConsoleColor.Green, $"Your favorite place is '{FavoritePlace}'");
                        break;
                    }
                    default:
                    {
                        Output.WriteLine(ConsoleColor.Red, $"Demo for {Enum.GetName(demo)} not implemented");
                        break;
                    }
                }

                if (!Input.ReadBool("More Input Demos?", true))
                {
                    break;
                }
            }

            Input.ReadString("Press [Enter] to navigate home");
            await Program.NavigateHome();
        }

        private static IEnumerable<(string name, TEnum value)> GetEnumNamesAndValues<TEnum>() where TEnum : struct, Enum
        {
            var names = Enum.GetNames<TEnum>();
            var values = Enum.GetValues<TEnum>();

            if (names.Length != values.Length)
            {
                throw new ArgumentException($"{nameof(Enum)}.{nameof(Enum.GetNames)}.{nameof(names.Length)} != {nameof(Enum)}.{nameof(Enum.GetValues)}.{nameof(values.Length)}", nameof(TEnum));
            }

            return names.Zip(values);
        }
    }

    enum Fruit
    {
        Apple,
        Banana,
        Coconut,
        Grape,
    }

    enum InputDemo
    {
        ReadOption,
        ReadEnum,
        ReadEnumWithDefault,
        MultiChoiceEnum,
        MultiChoiceEnumWithDefault,
        MultiChoiceMenuEnumWithAllDefault,
        MultiChoiceInt,
        String,
        StringWithDefault,
    }
}