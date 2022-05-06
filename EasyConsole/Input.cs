using System;
using EasyConsole.Types;

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
                Output.DisplayPrompt("Please enter an integer between {0} and {1} (inclusive):", min, max);
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

        public static IReadOnlyCollection<int> ReadMultiChoiceInt(string prompt, int min, int max)
        {
            Output.DisplayPrompt(prompt);
            return ReadMultiChoiceInt(min, max);
        }

        public static IReadOnlyCollection<int> ReadMultiChoiceInt(int min, int max)
        {
            while (true) // Loop indefinitely
            {
                var userInput = Console.ReadLine();
                if (userInput == null)
                {
                    throw new InvalidOperationException("Read null from Console.ReadLine()");
                }

                var values = userInput.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var results = Array.ConvertAll(values, s => int.TryParse(s, out var i) ? (int?)i : null);
                if (results.Any() && !results.Any(x => x is null || x < min || x > max))
                {
                    return Array.ConvertAll(results, x => x!.Value);
                }

                Output.DisplayPrompt($"Please enter a comma delimited list of integers between {min} and {max} (inclusive):");
            }
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

            var menu = new Menu();

            var choice = default(TEnum);
            foreach (var (name, value) in GetEnumNamesAndValues<TEnum>())
            {
                menu.Add(name, () =>
                {
                    choice = value;
                    return Task.CompletedTask;
                });
            }

            Output.WriteLine(prompt);
            await menu.Display();

            return choice;
        }

        public static IReadOnlyCollection<TEnum> ReadMultiChoiceEnum<TEnum>(string prompt) where TEnum : struct, Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type", nameof(TEnum));
            }

            var menu = new MultiChoiceMenu<TEnum>();

            foreach (var (name, value) in GetEnumNamesAndValues<TEnum>())
            {
                menu.Add(name, value);
            }

            Output.WriteLine(prompt);
            var choices = menu.Display();

            return choices;
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
}
