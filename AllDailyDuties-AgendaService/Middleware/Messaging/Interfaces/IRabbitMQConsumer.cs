﻿using RabbitMQ.Client;

namespace AllDailyDuties_AgendaService.Middleware.Messaging.Interfaces
{
    public interface IRabbitMQConsumer
    {
        public void ConsumeMessage<T>(IModel channel, string queue);



    }
}
