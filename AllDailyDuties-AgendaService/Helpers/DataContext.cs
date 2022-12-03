using AllDailyDuties_AgendaService.Models.Shared;
using AllDailyDuties_AgendaService.Models.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AllDailyDuties_AgendaService.Helpers
{
    public class DataContext : DbContext
    {
        private DbContextOptionsBuilder<DataContext> contextOptions;

        //protected readonly IConfiguration Configuration;
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }


        public DataContext(DbContextOptionsBuilder<DataContext> contextOptions)
        {
            this.contextOptions = contextOptions;
        }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
    }
}