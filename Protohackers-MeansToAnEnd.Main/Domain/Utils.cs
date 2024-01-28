namespace Protohackers_MeansToAnEnd.Main.Domain;
public static class Utils
{
    public static DateTime SecondsSinceUnixEpochToDateTime(int seconds)
    {
        return DateTime.UnixEpoch.AddSeconds(seconds);
    }

    public static int DateTimeToSecondsSinceUnixEpoch(DateTime date)
    {
        return (int)(date - DateTime.UnixEpoch).TotalSeconds;
    }

    public static int ToInt(ArraySegment<byte> bytes)
    {
        if (bytes.Count != 4)
            throw new ArgumentException();
        var val = 0;
        val |= bytes[3];
        val |= bytes[2] << 8;
        val |= bytes[1] << 16;
        val |= bytes[0] << 24;
        return val;
    }

    public static byte[] ToBytes(int val)
    {
        var bytes = BitConverter.GetBytes(val);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        
        return bytes;
    }
}