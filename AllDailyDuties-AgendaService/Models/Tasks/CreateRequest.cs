using AllDailyDuties_AgendaService.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace AllDailyDuties_AgendaService.Models.Tasks
{
    public class CreateRequest
    {   [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime ScheduledAt { get; set; }
        [Required]
        public List<User>? Users { get; set; }

    }
}
