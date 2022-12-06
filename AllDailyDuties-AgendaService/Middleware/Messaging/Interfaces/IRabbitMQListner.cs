namespace AllDailyDuties_AgendaService.Middleware.Messaging.Interfaces
{
    public interface IRabbitMQListner
    {
        object FetchObject(object obj);

        object DesObj(string obj);
    }
}
