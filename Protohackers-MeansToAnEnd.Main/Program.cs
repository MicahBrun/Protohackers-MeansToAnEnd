using Autofac;
using Autofac.Core;

using Protohackers_MeansToAnEnd.Main.Impl;
using Protohackers_MeansToAnEnd.Main.Interfaces;

namespace Protohackers_MeansToAnEnd.Main;
class Program
{
    public static void Main()
    {
        var container = Register();
        using (var scope = container.BeginLifetimeScope())
        {
            var service = scope.Resolve<IService>();
            service.Run();
        }
    }

    private static IContainer Register()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        builder.RegisterType<RequestCreator>().As<IRequestCreator>().SingleInstance();
        builder.RegisterType<RequestHandler>().As<IRequestHandler>().SingleInstance();
        builder.RegisterType<ServerManager>().As<IServerManager>().SingleInstance();
        builder.RegisterType<Impl.Service>().As<IService>().SingleInstance();

        return builder.Build();
    }
}