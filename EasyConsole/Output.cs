namespace EasyConsole
{
    public static class Output
    {
        public static void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

        public static void WriteLine(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public static void DisplayPrompt(string format, params object[] args)
        {
            const string promptEnd = ": ";
            if (format.EndsWith(promptEnd))
            {
                Console.Write(format.TrimStart(), args);
                return;
            }

            format = format.Trim().TrimEnd(':') + promptEnd;
            Console.Write(format, args);
        }
    }
}
