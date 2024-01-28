using Protohackers_MeansToAnEnd.Main.Interfaces;

namespace Protohackers_MeansToAnEnd.Main.Impl;
public class Service : IService
{
    private IServerManager _serverManager;
    public Service(IServerManager serverManager)
    {
        _serverManager = serverManager;
    }

    public void Run()
    {
        _serverManager.StartServer();

        Console.WriteLine("Press ENTER to exit.");
        Console.Read();
    }
}