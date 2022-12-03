using AllDailyDuties_AgendaService.Helpers;
using AllDailyDuties_AgendaService.Models.Tasks;
using AllDailyDuties_AgendaService.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AllDailyDuties_AgendaService.Repositories
{
    public class TaskItemRepo : ITaskItemRepo
    {
        private static IServiceProvider _provider;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TaskItemRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddAsync(CreateRequest entity)
        {
            try
            {
                var conn = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["default"];
                // Looked into overriding OnConfiguring method in DataContext but wasn't able to connect
                var contextOptions = new DbContextOptionsBuilder<DataContext>()
                    .UseMySql(conn, ServerVersion.AutoDetect(conn)).Options;
                var dbContext = new DataContext(contextOptions);
                var task = _mapper.Map<TaskItem>(entity);
                var result = await dbContext.AddAsync(task);
                await dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                // Any ConcurrencyException caught during updating records should be handled here (e.g. trying to update a deleted entry)
                return false;
            }
        }
    }
}
