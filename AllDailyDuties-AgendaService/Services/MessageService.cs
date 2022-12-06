using AllDailyDuties_AgendaService.Models.Shared;
using AllDailyDuties_AgendaService.Models.Tasks;
using AllDailyDuties_AgendaService.Repositories.Interfaces;
using AllDailyDuties_AgendaService.Services.Interfaces;
using Newtonsoft.Json;

namespace AllDailyDuties_AgendaService.Services
{
    public class MessageService<T> : IMessageService
    {
        private ITaskService _taskService;
        private ITaskItemRepo _repo;
        public MessageService(ITaskService taskService, ITaskItemRepo repo)
        {
            _taskService = taskService;
            _repo = repo;
        }
        public void CreateObject(T objectType, string message, string json)
        {
            // I know it's ugly, but it does the job
            if (objectType.GetType() == typeof(TaskItemMessage))
            {
                CreateNewTask(message, json);
            }
        }

        public void CreateNewTask(string message, string json)
        {
            TaskUser user = _taskService.GetTaskUser(message);
            TaskItemMessage taskItem = JsonConvert.DeserializeObject<TaskItemMessage>(json);
            Guid guid = Guid.NewGuid();

            CreateRequest item = new CreateRequest(guid, taskItem.Title, taskItem.CreatedAt, taskItem.ScheduledAt, user);
            _repo.AddAsync(item);
        }
    }
}
