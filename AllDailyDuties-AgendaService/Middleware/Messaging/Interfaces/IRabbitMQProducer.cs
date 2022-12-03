namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public interface IRabbitMQProducer
    {
        public void SendMessage<T>(T message, string queue, object obj);
    }
}
