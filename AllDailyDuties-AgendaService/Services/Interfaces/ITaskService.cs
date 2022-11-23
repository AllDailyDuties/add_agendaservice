using AllDailyDuties_AgendaService.Models.Tasks;

namespace AllDailyDuties_AgendaService.Services.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAll();
        TaskItem GetById(Guid id);
        void Create(CreateRequest model);
    }
}
