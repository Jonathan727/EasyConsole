namespace EasyConsole;

public interface IOption
{
    string Name { get; }

    public bool IsValueNull();
}