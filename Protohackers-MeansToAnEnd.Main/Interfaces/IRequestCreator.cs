using Protohackers_MeansToAnEnd.Main.Domain;

namespace Protohackers_MeansToAnEnd.Main.Interfaces;
public interface IRequestCreator
{
    public IRequest Create(byte[] byteRequest);
}