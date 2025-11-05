namespace RandomUserShifts.App.Cli;

public interface IConsoleWriter
{
    void WriteLine(string value);
}

public sealed class SystemConsoleWriter : IConsoleWriter
{
    public void WriteLine(string value) => Console.WriteLine(value);
}
