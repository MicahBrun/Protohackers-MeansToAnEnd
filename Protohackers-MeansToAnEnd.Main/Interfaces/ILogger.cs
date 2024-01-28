namespace Protohackers_MeansToAnEnd.Main.Interfaces;
public interface ILogger
{
    public void Error(Exception ex);
    public void Info(string message);
}