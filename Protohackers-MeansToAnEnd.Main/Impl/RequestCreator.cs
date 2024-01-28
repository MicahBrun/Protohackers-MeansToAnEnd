using Protohackers_MeansToAnEnd.Main.Domain;
using Protohackers_MeansToAnEnd.Main.Interfaces;

namespace Protohackers_MeansToAnEnd.Main.Impl;
public class RequestCreator : IRequestCreator
{
    public IRequest Create(byte[] byteRequest)
    {
        if (byteRequest.Length != 9)
            throw new ArgumentException($"Length of {nameof(byteRequest)} must be 9. Length was {byteRequest.Length}.");

        var requestType = Convert.ToChar(byteRequest[0]);
        switch (requestType)
        {
            case 'I':
                var timestampBytes = new ArraySegment<byte>(byteRequest, 1, 4);
                var timeStamp = Utils.SecondsSinceUnixEpochToDateTime(Utils.ToInt(timestampBytes));

                var priceBytes = new ArraySegment<byte>(byteRequest, 5, 4);
                var price = Utils.ToInt(priceBytes);

                return new Request.Insert
                {
                    Timestamp = timeStamp,
                    PriceInPennies = price,
                };
            case 'Q':
                var minTimeBytes = new ArraySegment<byte>(byteRequest, 1, 4);
                var minTime = Utils.SecondsSinceUnixEpochToDateTime(Utils.ToInt(minTimeBytes));

                var maxTimeBytes = new ArraySegment<byte>(byteRequest, 5, 4);
                var maxTime = Utils.SecondsSinceUnixEpochToDateTime(Utils.ToInt(maxTimeBytes));

                return new Request.Query
                {
                    MinTime = minTime,
                    MaxTime = maxTime,
                };
            default:
                throw new ArgumentException($"Request type not accepted.");
        }
    }
}