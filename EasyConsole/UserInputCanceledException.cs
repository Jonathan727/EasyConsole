using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConsole
{
    /// <summary>
    /// Indicates that the user canceled input. They can do this by pressing <kbd>Ctrl+Z</kbd> or <kbd>F6</kbd> (followed by <kbd>Enter</kbd> on Windows) when reading input from the console
    /// </summary>
    public class UserInputCanceledException : Exception
    {
    }
}
