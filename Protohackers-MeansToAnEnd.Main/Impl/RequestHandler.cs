using Protohackers_MeansToAnEnd.Main.Domain;
using Protohackers_MeansToAnEnd.Main.Interfaces;

namespace Protohackers_MeansToAnEnd.Main.Impl;
public class RequestHandler : IRequestHandler
{
    public int? HandleRequest(IRequest request, PriceStore priceStore)
    {

        switch (request)
        {
            case Request.Insert insert:
                priceStore.AddPriceAtTime(new Price 
                {
                    TimeStamp = insert.Timestamp, 
                    PriceInPennies = insert.PriceInPennies,
                });
                return null;
            case Request.Query query:
                return priceStore.GetAveragePricesBetweenTimes(query.MinTime, query.MaxTime);
            default:
                throw new NotSupportedException($"{request.GetType()} is not supported.");
        }
    }
}