﻿namespace EasyConsole
{
    /// <summary>
    /// Menu where the options perform some asynchronous task
    /// </summary>
    public class Menu : MenuBase<Func<Task>, Option, Task>
    {
        public override async Task Display()
        {
            ValidateOptions(true);

            for (var i = 0; i < Options.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
            }

            var choice = Input.ReadInt("Choose an option:", min: 1, max: Options.Count);

            await Options[choice - 1].Value();
        }

        public Menu Add(string name, Func<Task> callback)
        {
            Add(new Option(name, callback));
            return this;
        }
    }
}