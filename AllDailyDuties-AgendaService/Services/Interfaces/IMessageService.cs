namespace AllDailyDuties_AgendaService.Services.Interfaces
{
    public interface IMessageService
    {
        Task CreateObject<T>(string message, string json, string queue);
    }
}
