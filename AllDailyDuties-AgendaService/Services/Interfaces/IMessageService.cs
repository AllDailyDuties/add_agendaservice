namespace AllDailyDuties_AgendaService.Services.Interfaces
{
    public interface IMessageService
    {
        void CreateObject(object objectType, string message, string json);
    }
}
