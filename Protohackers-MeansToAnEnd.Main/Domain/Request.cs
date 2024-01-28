namespace Protohackers_MeansToAnEnd.Main.Domain
{
    public interface IRequest {}
    public static class Request
    {
        public record Insert : IRequest
        {
            public DateTime Timestamp { get; set; }
            public int PriceInPennies { get; set; }
        }
        public record Query : IRequest
        {
            public DateTime MinTime { get; set; }
            public DateTime MaxTime { get; set; }
        }
        
    }
}