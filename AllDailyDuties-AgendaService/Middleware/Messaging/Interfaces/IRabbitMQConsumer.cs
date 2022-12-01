using RabbitMQ.Client;

namespace AllDailyDuties_AgendaService.Middleware.Messaging.Interfaces
{
    public interface IRabbitMQConsumer
    {
        public void ConsumeMessage(IModel channel, string queue);



    }
}
