using AllDailyDuties_AgendaService.Models.Shared;
using AllDailyDuties_AgendaService.Models.Tasks;
using AllDailyDuties_AgendaService.Repositories.Interfaces;
using AllDailyDuties_AgendaService.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace AllDailyDuties_AgendaService.Services
{
    public class MessageService : IMessageService
    {
        private ITaskService _taskService;
        private ITaskItemRepo _repo;
        public CosmosClient _client;
        public MessageService(ITaskService taskService, ITaskItemRepo repo, CosmosClient client)
        {
            _taskService = taskService;
            _repo = repo;
            _client = client;
        }
        public async Task CreateObject<T>(string message, string json, string queue)
        {
            T myObject = JsonConvert.DeserializeObject<T>(json);

            CreateNewTaskAsync(message, myObject, queue);

        }


        public async Task CreateNewTaskAsync<T>(string message, T json, string queue)
        {
            string dbname = "AllDailyDuties";
            string containername = "AgendaService";
            dynamic obj;
            Database database = await _client.CreateDatabaseIfNotExistsAsync(dbname);
            Container container = database.GetContainer(containername);
            Guid guid = Guid.NewGuid();

            // I want to abstract this aswell
            switch (queue)
            {
                case "user_object":
                    TaskUser user = _taskService.GetTaskUser(message);
                    obj = new
                    {
                        id = guid,
                        Json = json,
                        User = user
                    };
                    break;

                default:
                    obj = new
                    {
                        id = guid,
                        Json = json
                    };
                    break;
            }

            //TaskItemMessage taskItem = JsonConvert.DeserializeObject<TaskItemMessage>(json.ToString());

            //CreateRequest item = new CreateRequest(guid, taskItem.Title, taskItem.Activity, taskItem.CreatedAt, taskItem.ScheduledAt, user);
            
            dynamic dbEntry = await container.CreateItemAsync<dynamic>(
                item: obj,
                partitionKey: new PartitionKey(guid.ToString()));

            //_repo.AddAsync(item);
        }
    }
}
