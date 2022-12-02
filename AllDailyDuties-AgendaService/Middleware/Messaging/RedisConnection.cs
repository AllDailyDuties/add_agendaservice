﻿using StackExchange.Redis;

namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> Lazy = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = { "localhost:6379" }
                });
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
