﻿using System.Globalization;

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

        internal static int ReadIntDoNotAppendDefaultToPrompt(string prompt, int min, int max, int @default)
        {
            if (@default > max || @default < min)
            {
                throw new ArgumentOutOfRangeException(nameof(@default), @default, "default value given is outside of range");
            }
            Output.DisplayPrompt(prompt);
            return ReadInt(min, max, @default);
        }

        public static int ReadInt(string prompt, int min, int max, int @default)
        {
            if (@default > max || @default < min)
            {
                throw new ArgumentOutOfRangeException(nameof(@default), @default, "default value given is outside of range");
            }

            Output.DisplayPrompt($"{prompt} [{@default}]:");
            return ReadInt(min, max, @default);
        }

        private static int ReadInt(int min, int max, int @default)
        {
            if (@default > max || @default < min)
            {
                throw new ArgumentOutOfRangeException(nameof(@default), @default, "default value given is outside of range");
            }

            while (true) // Loop indefinitely
            {
                var userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    return @default;
                }

                if (int.TryParse(userInput, out var result) && result >= min && result <= max)
                {
                    return result;
                }

                Output.DisplayPrompt($"Please enter an integer between {min} and {max} (inclusive) [{@default}]:");
            }
        }

        public static int ReadInt(string prompt, int @default)
        {
            Output.DisplayPrompt($"{prompt} [{@default}]:");
            return ReadInt(@default);
        }

        private static int ReadInt(int @default)
        {
            while (true) // Loop indefinitely
            {
                var userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    return @default;
                }

                if (int.TryParse(userInput, out var result))
                {
                    return result;
                }

                Output.DisplayPrompt($"Please enter an integer between {int.MinValue} and {int.MaxValue} (inclusive) [{@default}]:");
            }
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

        public static string ReadStringWithDefault(string prompt, string? @default)
        {
            if (string.IsNullOrWhiteSpace(@default))
            {
                return Input.ReadString($"{prompt}:") ?? throw new InvalidOperationException();
            }

            var sanitized = new string(@default.Where(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || ' ' == c).ToArray());
            Output.DisplayPrompt($"{prompt}:");
            var result = ReadLineWithDefaultText(sanitized);
            return result;
        }

        private static string ReadLineWithDefaultText(string @default)
        {
            var initialCursorLeft = Console.CursorLeft;
            Console.Write(@default);
            var chars = new List<char>();

            //sanitize
            @default = new string(@default.Where(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || ' ' == c).ToArray());

            if (!string.IsNullOrWhiteSpace(@default))
            {
                chars.AddRange(@default.ToCharArray());
            }

            while (true)
            {
                var info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Escape)
                {
                    while (Console.CursorLeft > initialCursorLeft)
                    {
                        chars.RemoveAt(chars.Count - 1);
                        Console.CursorLeft -= 1;
                        Console.Write(' ');
                        Console.CursorLeft -= 1;
                    }
                }
                if (info.Key == ConsoleKey.Backspace && Console.CursorLeft > initialCursorLeft)
                {
                    chars.RemoveAt(chars.Count - 1);
                    Console.CursorLeft -= 1;
                    Console.Write(' ');
                    Console.CursorLeft -= 1;

                }
                else if (info.Key == ConsoleKey.Enter)
                {
                    Console.Write(Environment.NewLine);
                    break;
                }
                //Here you need create own checking of symbols
                else if (char.IsLetterOrDigit(info.KeyChar) || char.IsPunctuation(info.KeyChar))
                {
                    Console.Write(info.KeyChar);
                    chars.Add(info.KeyChar);
                }
            }
            return new string(chars.ToArray());
        }

        public static bool ReadBool(string prompt, bool @default)
        {
            bool result;

            string? ReadStringLocal()
            {
                return Input.ReadString(@default ? $"{prompt} ([yes]/no):" : $"{prompt} ([no]/yes):");
            }

            for (var s = ReadStringLocal(); !TryParseBool(s, @default, out result); s = ReadStringLocal())
            {
                Console.WriteLine("Invalid Input");
            }

            return result;
        }

        private static bool TryParseBool(string? s, bool @default, out bool result)
        {
            if (s == null)
            {
                result = default;
                return false;
            }

            if (string.IsNullOrEmpty(s))
            {
                result = @default;
                return true;
            }

            if (s.StartsWith("y", StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
                return true;
            }

            if (s.StartsWith("n", StringComparison.InvariantCultureIgnoreCase))
            {
                result = false;
                return true;
            }

            result = default;
            return false;
        }

        public static DateTime ReadDateTime(string prompt, DateTime @default)
        {
            DateTime dateTime;
            while (true) // Loop indefinitely
            {
                var userInput = Input.ReadString($"{prompt} [{@default:o}]:");
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    dateTime = @default;
                    break;
                }

                if (DateTime.TryParse(userInput, null, DateTimeStyles.AssumeUniversal, out dateTime))
                {
                    break;
                }

                Output.WriteLine("Invalid Input");
            }

            return dateTime;
        }

        public static DateTimeOffset ReadDateTime(string prompt, DateTimeOffset @default)
        {
            DateTimeOffset dateTime;
            while (true) // Loop indefinitely
            {
                var userInput = Input.ReadString($"{prompt} [{@default:o}]:");
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    dateTime = @default;
                    break;
                }

                if (DateTimeOffset.TryParse(userInput, null, DateTimeStyles.AssumeUniversal, out dateTime))
                {
                    break;
                }

                Output.WriteLine("Invalid Input");
            }

            return dateTime;
        }

        public static DateTimeOffset? ReadDateTimeNullable(string prompt, DateTimeOffset? @default)
        {
            string? ReadStringLocal()
            {
                return Input.ReadString(@default.HasValue ? $"{prompt} ([{@default:o}]/no/none/null):" : $"{prompt} ([no/none/null]/date):");
            }

            DateTimeOffset? result;
            for (var s = ReadStringLocal(); !TryParseOptionalDateTime(s, @default, out result); s = ReadStringLocal())
            {
                Output.WriteLine("Invalid Input");
                Output.WriteLine($"You can type 'no' or 'none' or 'null' or enter a date/time like '{DateTimeOffset.UtcNow:d}' or '{DateTimeOffset.Now}'");
            }

            return result;
        }

        private static bool TryParseOptionalDateTime(string? userInput, DateTimeOffset? @default, out DateTimeOffset? result)
        {
            if (userInput == null)
            {
                result = @default;
                return false;
            }


            if (string.IsNullOrEmpty(userInput))
            {
                //accept default
                result = @default;
                return true;
            }

            if (userInput.StartsWith("y", StringComparison.InvariantCultureIgnoreCase))
            {
                //user has indicated they don't want null or none but hasn't specified a date/time
                //prompt for actual time

                //There is only a default in this prompt if the default value is not null
                var fullPrompt = @default.HasValue
                    ? $"What date/time?[{@default:o}]:"
                    : "What date/time?:";
                var userInput2 = Input.ReadString(fullPrompt);
                if (string.IsNullOrEmpty(userInput2))
                {
                    //This is valid iff there is only a default value (user already specified that they don't intend to enter null/none)
                    if (@default.HasValue)
                    {
                        result = @default;
                        return true;
                    }

                    Output.WriteLine($"Invalid Input. You can specify dates and times like '{DateTimeOffset.UtcNow:o}'. Time is assumed to be UTC unless otherwise specified.");
                    result = null;
                    return false;
                }

                if (DateTimeOffset.TryParse(userInput, null, DateTimeStyles.AssumeUniversal, out var dateTime2))
                {
                    result = dateTime2;
                    return true;
                }

                Output.WriteLine($"Invalid Input. You can specify dates and times like '{DateTimeOffset.UtcNow:o}'. Time is assumed to be UTC unless otherwise specified.");
            }

            if (userInput.StartsWith("n", StringComparison.InvariantCultureIgnoreCase))
            {
                //user selected null/none
                result = null;
                return true;
            }

            if (DateTimeOffset.TryParse(userInput, null, DateTimeStyles.AssumeUniversal, out var dateTime))
            {
                result = dateTime;
                return true;
            }

            result = @default;
            return false;
        }

        public static T ReadOption<T>(IEnumerable<T> values)
        {
            var menu = new ValueMenu<T>();
            var options = values.Select(x => new ValueOption<T>(x?.ToString() ?? "NULL", x));
            menu.AddRange(options);
            return menu.Display();
        }

        public static T ReadOption<T>(IEnumerable<ValueOption<T>> options)
        {
            var menu = new ValueMenu<T>();
            menu.AddRange(options);
            return menu.Display();
        }

        public static T? ReadOption<T>(IEnumerable<T?> values) where T : struct
        {
            var menu = new ValueMenu<T?>();
            var options = values.Select(x => new ValueOption<T?>(x.ToString() ?? "NULL", x));
            menu.AddRange(options);
            return menu.Display();
        }

        public static T? ReadOption<T>(IEnumerable<ValueOption<T?>> options) where T : struct
        {
            var menu = new ValueMenu<T?>();
            menu.AddRange(options);
            return menu.Display();
        }

        public static TEnum ReadEnum<TEnum>(string prompt) where TEnum : struct, Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type", nameof(TEnum));
            }

            var menu = new ValueMenu<TEnum>();

            foreach (var (name, value) in GetEnumNamesAndValues<TEnum>())
            {
                menu.Add(name, value);
            }

            Output.WriteLine(prompt);
            return menu.Display();
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
