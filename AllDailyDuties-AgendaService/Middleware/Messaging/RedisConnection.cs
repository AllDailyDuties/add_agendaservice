using StackExchange.Redis;

namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> Lazy = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("localhost:6379,password=1_1qwerty");
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return Lazy.Value;
            }
        }
    }
}
