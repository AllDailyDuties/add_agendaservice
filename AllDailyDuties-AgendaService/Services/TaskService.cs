using AllDailyDuties_AgendaService.Helpers;
using AllDailyDuties_AgendaService.Models.Tasks;
using AllDailyDuties_AgendaService.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AllDailyDuties_AgendaService.Services
{
    public class TaskService : ITaskService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public TaskService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<TaskItem> GetAll()
        {
            return _context.Tasks;
        }

        public TaskItem GetById(Guid id)
        {
            return getUser(id);
        }
        public void Create(CreateRequest model)
        {
            // map model to new user object
            var task = _mapper.Map<TaskItem>(model);

            // save user.
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        // Helper method(s)
        private TaskItem getUser(Guid id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null) throw new KeyNotFoundException("Task not found");
            return task;
        }
    }
}
