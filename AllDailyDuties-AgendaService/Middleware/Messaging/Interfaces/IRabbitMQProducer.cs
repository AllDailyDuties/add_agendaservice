namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public interface IRabbitMQProducer
    {
        public string SendMessage<T>(T message, string queue);
    }
}
