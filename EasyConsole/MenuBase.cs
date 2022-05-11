using EasyConsole.Types;

namespace EasyConsole
{
    /// <summary>
    /// Generic base class for a menu that shows the user a list of options and processes the user's selection
    /// </summary>
    /// <typeparam name="TValue">The type of <see cref="ValueOption{T}.Value"/> in each element of <see cref="Options"/></typeparam>
    /// <typeparam name="TOption">A <see cref="ValueOption{T}"/> containing <typeparamref name="TValue"/></typeparam>
    /// <typeparam name="TSelection">The type returned by <see cref="DisplayPrompt"/> indicating user's choice(s). Could for example be as single <typeparamref name="TSelection"/> or an <see cref="IEnumerable{T}"/> of <typeparamref name="TSelection"/>. The result is handled by <see cref="OnUserAnsweredPrompt"/></typeparam>
    /// <typeparam name="TReturn">The type that will be returned by <see cref="OnUserAnsweredPrompt"/> and <see cref="Display"/></typeparam>
    public abstract class MenuBase<TValue, TOption, TSelection, TReturn> where TOption : ValueOption<TValue>
    {
        private readonly string _promptText = "Choose an option";
        private readonly TOption? _defaultOption;

        protected string PromptText
        {
            get => DefaultOption != null ? $"{_promptText}  [{DefaultOptionNumber}. {DefaultOption.Name}]" : _promptText;
            init => _promptText = value;
        }

        protected List<TOption> Options { get; } = new();
        protected TOption? DefaultOption
        {
            get => _defaultOption;
            init
            {
                if (value != null && !Options.Contains(value))
                {
                    Options.Add(value);
                }
                _defaultOption = value;
            }
        }

        protected bool HasDefaultOption => DefaultOption != null;
        protected int DefaultOptionNumber
        {
            get
            {
                var defaultOptionIndex = DefaultOption != null ? Options.IndexOf(DefaultOption) : throw new InvalidOperationException("No Default Option");
                if (defaultOptionIndex == -1)
                {
                    throw new InvalidOperationException($"{nameof(DefaultOption)} not found in {nameof(Options)}");
                }
                return defaultOptionIndex + 1;
            }
        }
        protected bool AllowNullOptionValues { get; }

        protected MenuBase(TOption? defaultOption = null, bool allowNullOptionValues = false)
        {
            DefaultOption = defaultOption;
            AllowNullOptionValues = allowNullOptionValues;
        }

        public TReturn Display()
        {
            ValidateOptions(!AllowNullOptionValues);

            DisplayOptions();

            var chosenOption = DisplayPrompt();

            return OnUserAnsweredPrompt(chosenOption);
        }

        protected void ValidateOptions(bool checkForNull)
        {
            if (!Options.Any())
            {
                throw new InvalidOperationException($"No items in {nameof(Options)}.");
            }
            if (checkForNull && Options.Any(x => x.Value is null))
            {
                throw new InvalidOperationException($"{typeof(TOption)}.{nameof(ValueOption<TValue>.Value)} was null or is not set in one or more {nameof(Options)}");
            }
            if (DefaultOption != null && !Options.Contains(DefaultOption))
            {
                throw new InvalidOperationException($"{nameof(DefaultOption)} not found in {nameof(Options)}");
            }
        }

        protected void DisplayOptions()
        {
            for (var i = 0; i < Options.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
            }
        }

        /// <summary>
        /// Print the prompt message and return the user's selection.
        /// </summary>
        /// <returns></returns>
        protected abstract TSelection DisplayPrompt();

        /// <summary>
        /// Called when the user has answered the prompt from <see cref="DisplayPrompt"/>. Use this method to process the selection into the return type, <typeparamref name="TReturn"/>.
        /// </summary>
        /// <param name="selection">The selection made by the user.</param>
        /// <returns></returns>
        protected abstract TReturn OnUserAnsweredPrompt(TSelection selection);

        /// <summary>
        /// Getter that translates 1-indexed option numbers to the zero-indexed <see cref="Options"/> list
        /// </summary>
        /// <param name="optionNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected TOption GetOption(int optionNumber)
        {
            if (optionNumber > Options.Count || optionNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(optionNumber));
            }

            return Options[optionNumber - 1];
        }

        protected IntRange GetOptionRange()
        {
            if (!Options.Any())
            {
                throw new InvalidOperationException($"No items in {nameof(Options)}.");
            }

            return new IntRange(1, Options.Count);
        }

        public MenuBase<TValue, TOption, TSelection, TReturn> Add(TOption option)
        {
            Options.Add(option);
            return this;
        }

        public MenuBase<TValue, TOption, TSelection, TReturn> AddRange(IEnumerable<TOption> options)
        {
            Options.AddRange(options);
            return this;
        }

        public bool Contains(string option)
        {
            return Options.FirstOrDefault(op => op.Name.Equals(option)) != null;
        }
    }
}
