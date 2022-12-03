using AllDailyDuties_AgendaService.Helpers;
using AllDailyDuties_AgendaService.Models.Tasks;
using AllDailyDuties_AgendaService.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
                var connectionstring = "server=localhost; port=3306; database=DailyDuties-AgendaService; user=root; password=1_1qwerty";
                var contextOptions = new DbContextOptionsBuilder<DataContext>()
                    .UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring)).Options;
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
