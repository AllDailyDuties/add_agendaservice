namespace AllDailyDuties_AgendaService.Services.Interfaces
{
    public interface IMessageService
    {
        void CreateObject<T>(string message, string json, string queue);
    }
}
