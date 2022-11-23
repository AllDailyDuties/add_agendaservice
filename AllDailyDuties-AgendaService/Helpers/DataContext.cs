using AllDailyDuties_AgendaService.Models.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AllDailyDuties_AgendaService.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to mysql with connection string from app settings
            var connectionString = Configuration.GetConnectionString("Default");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}