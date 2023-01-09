using AllDailyDuties_AgendaService.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace AllDailyDuties_AgendaService.Models.Tasks
{
    public class CreateRequest
    {   //Small i for cosmosDb
        public Guid id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Activity { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime ScheduledAt { get; set; }
        [Required]
        public TaskUser User { get; set; }
        public CreateRequest(Guid _id, string _title, string _activity, DateTime _createdAt, DateTime _scheduledAt, TaskUser _user)
        {
            id = _id;
            Title = _title;
            Activity = _activity;
            Created = _createdAt;
            ScheduledAt = _scheduledAt;
            User = _user;
        }

    }
}
