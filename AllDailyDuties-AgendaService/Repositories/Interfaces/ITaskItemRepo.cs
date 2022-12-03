using AllDailyDuties_AgendaService.Models.Tasks;

namespace AllDailyDuties_AgendaService.Repositories.Interfaces
{
    public interface ITaskItemRepo
    {
        Task<bool> AddAsync(CreateRequest entity);
    }
}
