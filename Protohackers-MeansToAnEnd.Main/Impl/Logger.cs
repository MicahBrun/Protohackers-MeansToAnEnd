using Protohackers_MeansToAnEnd.Main.Interfaces;

namespace Protohackers_MeansToAnEnd.Main.Impl;
public class Logger : ILogger
{
    public void Error(Exception ex)
    {
        Console.WriteLine(ex);
    }

    public void Info(string message)
    {
        Console.WriteLine(message);
    }
}