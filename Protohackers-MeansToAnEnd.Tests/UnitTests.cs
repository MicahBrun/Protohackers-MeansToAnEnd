using NUnit.Framework;
using Protohackers_MeansToAnEnd.Main.Impl;
using Protohackers_MeansToAnEnd.Main.Domain;
using System.Net.Sockets;
using System.Net;

namespace Protohackers_MeansToAnEnd.Test;

public class UnitTests
{
    [Test]
    public void TestRequestHandlerReturnsAverageBetweenDates()
    {
        var priceStore = new PriceStore();
        var rh = new RequestHandler();

        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 1), PriceInPennies = 100}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 2), PriceInPennies = 400}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 3), PriceInPennies = 100}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 4), PriceInPennies = 10000}, priceStore);
        var average = rh.HandleRequest(new Request.Query() {MinTime = new DateTime(2023, 1, 1), MaxTime = new DateTime(2023, 1, 3)}, priceStore);

        Assert.That(average, Is.EqualTo(200));
    }
    [Test]
    public void TestRequestHandlerNoRequestsReturns0()
    {
        var priceStore = new PriceStore();
        var rh = new RequestHandler();

        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 1), PriceInPennies = 100}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 2), PriceInPennies = 400}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 3), PriceInPennies = 100}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 4), PriceInPennies = 10000}, priceStore);
        var average = rh.HandleRequest(new Request.Query() {MinTime = new DateTime(2021, 1, 1), MaxTime = new DateTime(2022, 1, 3)}, priceStore);

        Assert.That(average, Is.EqualTo(0));
    }
    [Test]
    public void TestRequestHandlerMaxDateBeforeMinDateReturns0()
    {
        var priceStore = new PriceStore();
        var rh = new RequestHandler();

        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 1), PriceInPennies = 100}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 2), PriceInPennies = 400}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 3), PriceInPennies = 100}, priceStore);
        rh.HandleRequest(new Request.Insert() {Timestamp = new DateTime (2023, 1, 4), PriceInPennies = 10000}, priceStore);
        var average = rh.HandleRequest(new Request.Query() {MinTime = new DateTime(2024, 1, 1), MaxTime = new DateTime(2023, 1, 1)}, priceStore);

        Assert.That(average, Is.EqualTo(0));
    }
    [Test]
    public void TestRequestCreatorReadsCorrectlyInsert()
    {
        var date = new DateTime(2023, 11, 9);

        var price = 200;
        var bytesI = Convert.ToByte('I');

        var bytesDate = Utils.ToBytes((int)(date - DateTime.UnixEpoch).TotalSeconds);
        var bytesPrice = Utils.ToBytes(price);

        var rc = new RequestCreator();
        var request = rc.Create(new byte[] {bytesI}.Concat(bytesDate).Concat(bytesPrice).ToArray());
        var requestExpected = new Request.Insert
        {
            Timestamp = date,
            PriceInPennies = price,
        };

        Assert.That(request, Is.EqualTo(requestExpected));
    }
    [Test]
    public void TestRequestCreatorReadsCorrectlyQuery()
    {
        var minTime = new DateTime(2023, 11, 9);
        var maxTime = new DateTime(2023, 11, 30);

        var bytesI = Convert.ToByte('Q');

        var bytesMinTime = Utils.ToBytes((int)(minTime - DateTime.UnixEpoch).TotalSeconds);
        var bytesMaxTime = Utils.ToBytes((int)(maxTime - DateTime.UnixEpoch).TotalSeconds);

        var rc = new RequestCreator();
        var request = rc.Create(new byte[] {bytesI}.Concat(bytesMinTime).Concat(bytesMaxTime).ToArray());
        var requestExpected = new Request.Query
        {
            MinTime = minTime,
            MaxTime = maxTime,
        };

        Assert.That(request, Is.EqualTo(requestExpected));
    }
    [Test]
    public void TestTcp()
    {
        var client = new TcpClient();
        var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
        client.Connect(ipEndPoint);

        var stream = client.GetStream();
        
        var date = new DateTime(2023, 11, 9);

        var price = 200;
        var bytesI = Convert.ToByte('I');

        var bytesDate = Utils.ToBytes((int)(date - DateTime.UnixEpoch).TotalSeconds);
        var bytesPrice = Utils.ToBytes(price);

        var rc = new RequestCreator();
        var request = new byte[] {bytesI}.Concat(bytesDate).Concat(bytesPrice).ToArray();

        stream.Write(request);
        
        var minTime = new DateTime(2023, 11, 9);
        var maxTime = new DateTime(2023, 11, 30);

        var bytesQ = Convert.ToByte('Q');

        var bytesMinTime = Utils.ToBytes((int)(minTime - DateTime.UnixEpoch).TotalSeconds);
        var bytesMaxTime = Utils.ToBytes((int)(maxTime - DateTime.UnixEpoch).TotalSeconds);

        var requestQ = new byte[] {bytesQ}.Concat(bytesMinTime).Concat(bytesMaxTime).ToArray();
        var response = new byte[4];

        stream.Write(requestQ);

        stream.Read(response);

        var amount = Utils.ToInt(response);
    }
}