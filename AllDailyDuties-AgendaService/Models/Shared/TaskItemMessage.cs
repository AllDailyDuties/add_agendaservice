namespace AllDailyDuties_AgendaService.Models.Shared
{
    public class TaskItemMessage
    {
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Activity { get; set; }
    }
}
