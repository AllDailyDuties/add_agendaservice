namespace AllDailyDuties_AgendaService.Models.Shared
{
    public class TaskUser
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

    }
}
