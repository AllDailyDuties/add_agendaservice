using AllDailyDuties_AgendaService.Helpers;
using AllDailyDuties_AgendaService.Models.Shared;
using AllDailyDuties_AgendaService.Models.Tasks;
using AllDailyDuties_AgendaService.Repositories;
using AllDailyDuties_AgendaService.Repositories.Interfaces;
using AllDailyDuties_AgendaService.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Dynamic;

namespace AllDailyDuties_AgendaService.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private ITaskItemRepo _repo;


        public TaskService(IConfiguration config, DataContext context, IMapper mapper, ITaskItemRepo repo)
        {
            _context = context;
            _mapper = mapper;
            _repo = repo;
        }

        public IEnumerable<TaskItem> GetAll()
        {
            return _context.Tasks;
        }

        public TaskItem GetById(Guid id)
        {
            return getUser(id);
        }

        public TaskUser GetTaskUser(string message)
        {
            TaskUser user = JsonConvert.DeserializeObject<TaskUser>(message);
            return user;

        }

        public TaskUser Return(TaskUser user)
        {
            return user;
        }
        public async void Create(CreateRequest model)
        {
            // map model to new object
            var task = _mapper.Map<TaskItem>(model);

            // save user.
            _context.Tasks.Add(task);
            _context.SaveChanges();

        }
        public async Task<bool> CreateNewTask(CreateRequest model)
        {
            await _repo.AddAsync(model);
            return true;
        }


        public async Task<bool> AddAsync(CreateRequest entity)
        {
            
            try
            {
                var task = _mapper.Map<TaskItem>(entity);
                var result = await _context.AddAsync(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                // Any ConcurrencyException caught during updating records should be handled here (e.g. trying to update a deleted entry)
                return false;
            }
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
