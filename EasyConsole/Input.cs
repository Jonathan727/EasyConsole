using System;

namespace EasyConsole
{
    public static class Input
    {
        public static int ReadInt(string prompt, int min, int max)
        {
            Output.DisplayPrompt(prompt);
            return ReadInt(min, max);
        }

        public static int ReadInt(int min, int max)
        {
            var value = ReadInt();

            while (value < min || value > max)
            {
                Output.DisplayPrompt("Please enter an integer between {0} and {1} (inclusive)", min, max);
                value = ReadInt();
            }

            return value;
        }

        public static int ReadInt()
        {
            var input = Console.ReadLine();
            int value;

            while (!int.TryParse(input, out value))
            {
                Output.DisplayPrompt("Please enter an integer");
                input = Console.ReadLine();
            }

            return value;
        }

        public static string? ReadString(string prompt)
        {
            Output.DisplayPrompt(prompt);
            return Console.ReadLine();
        }

        public static async Task<TEnum> ReadEnum<TEnum>(string prompt) where TEnum : struct, Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type", nameof(TEnum));
            }

            Output.WriteLine(prompt);
            var menu = new Menu();

            var names = Enum.GetNames<TEnum>();
            var values = Enum.GetValues<TEnum>();

            if (names.Length != values.Length)
            {
                throw new ArgumentException($"{nameof(Enum)}.{nameof(Enum.GetNames)}.{nameof(names.Length)} != {nameof(Enum)}.{nameof(Enum.GetValues)}.{nameof(values.Length)}", nameof(TEnum));
            }

            var choice = default(TEnum);
            foreach (var (name, value) in names.Zip(values))
            {
                menu.Add(name, () =>
                {
                    choice = value;
                    return Task.CompletedTask;
                });
            }
            await menu.Display();

            return choice;
        }
    }
}
