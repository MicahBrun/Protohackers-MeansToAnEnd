using Protohackers_MeansToAnEnd.Main.Domain;
using Protohackers_MeansToAnEnd.Main.Interfaces;

namespace Protohackers_MeansToAnEnd.Main.Domain;

public class PriceStore
{
    private List<Price> _priceChanges = new();

    public void AddPriceAtTime(Price priceChange)
    {
        lock (_priceChanges)
        {
            if (priceChange.PriceInPennies == 0)
                return;

            _priceChanges.Add(priceChange);
        }
    }

    public int GetAveragePricesBetweenTimes(DateTime dateFrom, DateTime dateTo)
    {
        if (dateTo < dateFrom)
            return 0;

        var average = _priceChanges
            .Where(pc => dateFrom <= pc.TimeStamp && pc.TimeStamp <= dateTo)
            .Select(pc => pc.PriceInPennies)
            .DefaultIfEmpty(0)
            .Average();
        return (int)Math.Round(average);
    }
}