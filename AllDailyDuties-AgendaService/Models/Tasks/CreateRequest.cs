using AllDailyDuties_AgendaService.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace AllDailyDuties_AgendaService.Models.Tasks
{
    public class CreateRequest
    {   
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime ScheduledAt { get; set; }
        [Required]
        public TaskUser User { get; set; }
        public CreateRequest(Guid _id, string _title, DateTime _createdAt, DateTime _scheduledAt, TaskUser _user)
        {
            Id = _id;
            Title = _title;
            Created = _createdAt;
            ScheduledAt = _scheduledAt;
            User = _user;
        }

    }
}
