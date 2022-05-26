namespace EasyConsole;

/// <summary>
/// Similar to <see cref="MultiChoiceMenu{T}"/>, but allows each option to refer to one or more <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class MultiChoiceMultiValueMenu<T> : MultiChoiceMenuBase<T, MultiValueOption<T>, IReadOnlyCollection<T>>
{
    public MultiChoiceMultiValueMenu()
    {
    }

    public MultiChoiceMultiValueMenu(string? prompt, string defaultOption, params T[] defaultValues) : this(prompt, new MultiValueOption<T>(defaultOption, defaultValues))
    {
    }

    public MultiChoiceMultiValueMenu(string defaultOption, params T[] defaultValues) : this(null, new MultiValueOption<T>(defaultOption, defaultValues))
    {
    }

    public MultiChoiceMultiValueMenu(string? prompt, MultiValueOption<T> defaultOption) : base(prompt)
    {
        DefaultOption = defaultOption;
    }

    protected override IReadOnlyCollection<T> OnUserAnsweredPrompt(IEnumerable<MultiValueOption<T>> selection)
    {
        return selection.SelectMany(x => x.Value).ToList();
    }
    public void Add(string name, T[] values)
    {
        Add(new MultiValueOption<T>(name, values));
    }
}