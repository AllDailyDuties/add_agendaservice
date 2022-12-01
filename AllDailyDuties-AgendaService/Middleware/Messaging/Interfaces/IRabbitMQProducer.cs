namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public interface IRabbitMQProducer
    {
        public void SendMessage<T>(T message);
    }
}
