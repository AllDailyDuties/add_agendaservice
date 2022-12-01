using AllDailyDuties_AgendaService.Models.Shared;
using AllDailyDuties_AgendaService.Models.Tasks;

namespace AllDailyDuties_AgendaService.Services.Interfaces
{
    public interface ITaskService
    {
        public TaskUser GetTaskUser(string message);
        IEnumerable<TaskItem> GetAll();
        TaskItem GetById(Guid id);
        void Create(CreateRequest model);
    }
}
