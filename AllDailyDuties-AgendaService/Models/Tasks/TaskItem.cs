using AllDailyDuties_AgendaService.Models.Shared;

namespace AllDailyDuties_AgendaService.Models.Tasks
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime ScheduledAt { get; set; }
        public List<User>? Users { get; set; }
    }
}
