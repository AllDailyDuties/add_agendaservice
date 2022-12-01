using AllDailyDuties_AgendaService.Models.Shared;

namespace AllDailyDuties_AgendaService.Models.Tasks
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime ScheduledAt { get; set; }
        public TaskUser User { get; set; }

        public TaskItem()
        {

        }
        public TaskItem(Guid _id, string _title, DateTime _created, DateTime _scheduledAt, TaskUser _user)
        {
            Id = _id;
            Title = _title;
            Created = _created;
            ScheduledAt = _scheduledAt;
            User = _user;
        }

        public override string ToString()
        {
            return "id:" + Id + " Title: " + Title + " User: " + User.Username.ToString();
        }
    }
}
