using System.Globalization;
using EasyConsole.Types;

namespace EasyConsole
{
    public static class Input
    {

        #region Int

        public static int ReadInt(string prompt, int min, int max)
        {
            Output.DisplayPrompt(prompt);
            return ReadInt(min, max);
        }

        public static int ReadInt(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException($"{nameof(min)} must be <= {nameof(max)}. {nameof(min)} was {min:N0}; {nameof(max)} was {max:N0}");
            }
            return ReadInt(new IntRange(min, max));
        }

        public static int ReadInt(string prompt, IntRange range)
        {
            Output.DisplayPrompt(prompt);
            return ReadInt(range.Min, range.Max);
        }

        public static int ReadInt(IntRange range)
        {
            while (true) // Loop indefinitely
            {
                var value = ReadInt();

                if (range.IsInside(value))
                {
                    return value;
                }

                Output.DisplayPrompt("Please enter an integer between {0} and {1} (inclusive):", range.Min, range.Max);
            }
        }

        public static int ReadInt()
        {
            while (true) // Loop indefinitely
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    throw new UserInputCanceledException();
                }

                if (int.TryParse(input, out var value))
                {
                    return value;
                }

                Output.DisplayPrompt("Please enter an integer");
            }
        }

        internal static int ReadIntDoNotAppendDefaultToPrompt(string prompt, int min, int max, int @default)
        {
            if (min > max)
            {
                throw new ArgumentException($"{nameof(min)} must be <= {nameof(max)}. {nameof(min)} was {min:N0}; {nameof(max)} was {max:N0}");
            }
            return ReadIntDoNotAppendDefaultToPrompt(prompt, new IntRange(min, max), @default);
        }

        internal static int ReadIntDoNotAppendDefaultToPrompt(string prompt, IntRange range, int @default)
        {
            Output.DisplayPrompt(prompt);
            return ReadInt(range, @default);
        }

        public static int ReadInt(string prompt, int min, int max, int @default)
        {
            if (min > max)
            {
                throw new ArgumentException($"{nameof(min)} must be <= {nameof(max)}. {nameof(min)} was {min:N0}; {nameof(max)} was {max:N0}");
            }
            Output.DisplayPrompt($"{prompt} [{@default}]:");
            return ReadInt(new IntRange(min, max), @default);
        }

        public static int ReadInt(string prompt, IntRange range, int @default)
        {
            Output.DisplayPrompt($"{prompt} [{@default}]:");
            return ReadInt(range, @default);
        }

        private static int ReadInt(IntRange range, int @default)
        {
            if (range.IsOutside(@default))
            {
                throw new ArgumentOutOfRangeException(nameof(@default), @default, $"default value given is outside of range. {nameof(range)} was {range}");
            }

            while (true) // Loop indefinitely
            {
                var userInput = Console.ReadLine();
                if (userInput == null)
                {
                    throw new UserInputCanceledException();
                }
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    return @default;
                }

                if (int.TryParse(userInput, out var result) && range.IsInside(result))
                {
                    return result;
                }

                Output.DisplayPrompt($"Please enter an integer between {range.Min} and {range.Max} (inclusive) [{@default}]:");
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
                if (userInput == null)
                {
                    throw new UserInputCanceledException();
                }
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
            if (min > max)
            {
                throw new ArgumentException($"{nameof(min)} must be <= {nameof(max)}. {nameof(min)} was {min:N0}; {nameof(max)} was {max:N0}");
            }
            return ReadMultiChoiceInt(new IntRange(min, max));
        }

        public static IReadOnlyCollection<int> ReadMultiChoiceInt(IntRange range)
        {
            while (true) // Loop indefinitely
            {
                var userInput = Console.ReadLine();
                if (userInput == null)
                {
                    throw new UserInputCanceledException();
                }

                var values = userInput.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var results = Array.ConvertAll(values, s => int.TryParse(s, out var i) ? (int?)i : null);
                if (results.Any() && results.All(x => x is not null && range.IsInside(x.Value)))
                {
                    return Array.ConvertAll(results, x => x!.Value);
                }

                Output.DisplayPrompt($"Please enter a comma delimited list of integers between {range.Min} and {range.Max} (inclusive):");
            }
        }

        #endregion

        #region String

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns><see langword="null" /> if <kbd>Ctrl+Z</kbd> (followed by <kbd>Enter</kbd> on Windows) is pressed when the method is reading input from the console</returns>
        /// <exception cref="UserInputCanceledException"></exception>
        public static string ReadString(string prompt)
        {
            Output.DisplayPrompt(prompt);
            return Console.ReadLine() ?? throw new UserInputCanceledException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="default">A single line string to use as default. If <see langword="null" />, then this parameter is ignored and the behavior is the same as <see cref="ReadString(string)"/></param>
        /// <returns></returns>
        /// <exception cref="UserInputCanceledException"></exception>
        public static string ReadStringWithDefault(string prompt, string? @default)
        {
            if (string.IsNullOrWhiteSpace(@default))
            {
                return ReadString(prompt);
            }

            var sanitized = new string(@default.Where(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || ' ' == c).ToArray());
            Output.DisplayPrompt(prompt);
            var result = ReadLineWithDefaultText(sanitized);
            return result ?? throw new UserInputCanceledException();
        }

        private static string? ReadLineWithDefaultText(string @default)
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
                    return new string(chars.ToArray());
                }
                //CTRL+Z + <enter> would normally cause Console.ReadLine() to return null. Do the in this method.
                else if (info.Key == ConsoleKey.Z && info.Modifiers == ConsoleModifiers.Control)
                {
                    return null;
                }
                //Check if the character is allowed
                else if (char.IsLetterOrDigit(info.KeyChar) || char.IsPunctuation(info.KeyChar))
                {
                    Console.Write(info.KeyChar);
                    chars.Add(info.KeyChar);
                }
            }
        }

        #endregion

        #region Bool

        public static bool ReadBool(string prompt, bool @default)
        {
            bool result;

            string ReadStringLocal()
            {
                return ReadString(@default ? $"{prompt} ([yes]/no):" : $"{prompt} ([no]/yes):");
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
                throw new UserInputCanceledException();
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

            result = @default;
            return false;
        }

        #endregion

        #region DateTime[Offset]

        public static DateTime ReadDateTime(string prompt, DateTime @default)
        {
            while (true) // Loop indefinitely
            {
                var userInput = ReadString($"{prompt} [{@default:o}]:");
                if (userInput == null)
                {
                    throw new UserInputCanceledException();
                }
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    return @default;
                }

                if (DateTime.TryParse(userInput, null, DateTimeStyles.AssumeUniversal, out var dateTime))
                {
                    return dateTime;
                }

                Output.WriteLine("Invalid Input");
            }
        }

        public static DateTimeOffset ReadDateTime(string prompt, DateTimeOffset @default)
        {
            while (true) // Loop indefinitely
            {
                var userInput = ReadString($"{prompt} [{@default:o}]:");
                if (userInput == null)
                {
                    throw new UserInputCanceledException();
                }
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    return @default;
                }

                if (DateTimeOffset.TryParse(userInput, null, DateTimeStyles.AssumeUniversal, out var dateTime))
                {
                    return dateTime;
                }

                Output.WriteLine("Invalid Input");
            }
        }

        /// <summary>
        /// Prompts user for a date time. If the user enters 'no/none/null' this method returns <see langword="null"/>.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="default">Default value to return when pressed enter without typing anything</param>
        /// <returns></returns>
        public static DateTimeOffset? ReadDateTimeNullable(string prompt, DateTimeOffset? @default)
        {
            string ReadStringLocal()
            {
                return ReadString(@default.HasValue ? $"{prompt} ([{@default:o}]/no/none/null):" : $"{prompt} ([no/none/null]/date):");
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
                throw new UserInputCanceledException();
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
                var userInput2 = ReadString(fullPrompt);
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

                if (DateTimeOffset.TryParse(userInput2, null, DateTimeStyles.AssumeUniversal, out var dateTime2))
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

        #endregion

        #region Generic Options

        public static T ReadOption<T>(IEnumerable<T> values)
        {
            var options = values.Select(x => new ValueOption<T>(x?.ToString() ?? "NULL", x));
            return ReadOption(options);
        }

        public static T ReadOption<T>(IEnumerable<ValueOption<T>> options)
        {
            var menu = new ValueMenu<T>();
            menu.AddRange(options);
            return menu.Display();
        }

        public static T? ReadOption<T>(IEnumerable<T?> values) where T : struct
        {
            var options = values.Select(x => new ValueOption<T?>(x.ToString() ?? "NULL", x));
            return ReadOption(options);
        }

        public static T? ReadOption<T>(IEnumerable<ValueOption<T?>> options) where T : struct
        {
            var menu = new ValueMenu<T?>();
            menu.AddRange(options);
            return menu.Display();
        }

        #endregion

        #region Enum

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

        public static TEnum ReadEnum<TEnum>(string prompt, TEnum @default) where TEnum : struct, Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type", nameof(TEnum));
            }

            var menu = new ValueMenuWithDefault<TEnum>(prompt, Enum.GetName(@default) ?? @default.ToString(), @default);

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

        #endregion
    }
}
