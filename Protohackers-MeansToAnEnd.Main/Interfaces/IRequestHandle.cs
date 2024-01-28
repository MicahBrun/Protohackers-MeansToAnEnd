using Protohackers_MeansToAnEnd.Main.Domain;

namespace Protohackers_MeansToAnEnd.Main.Interfaces;
public interface IRequestHandler
{
    public int? HandleRequest(IRequest request, PriceStore priceStore);
}