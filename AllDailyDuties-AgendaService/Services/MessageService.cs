using AllDailyDuties_AgendaService.Middleware.Messaging;
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
        private CosmosClient _client;
        private readonly IRabbitMQProducer _rabbit;
        public MessageService(ITaskService taskService, ITaskItemRepo repo, CosmosClient client, IRabbitMQProducer rabbit)
        {
            _taskService = taskService;
            _repo = repo;
            _client = client;
            _rabbit = rabbit;
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
            //string dateQuery = string.Format("SELECT c.User.Username, c.User.Email, c.Json.Title, c.Json.Activity, c.Json.ScheduledAt FROM c WHERE c.Json.Activity = {0} " +
             //   "AND c.User.Email != {1} AND DateTimeDiff(\"hh\", c.Json.ScheduledAt, {2}) = 0", );



            switch (queue)
            {
                case "user_object":
                    TaskUser user = _taskService.GetTaskUser(message);
                    obj = new
                    {
                        id = guid,
                        Json = json,
                        User = user,
                        Status = "unassigned"
                    };
                    // Initial idea is to check if there are >4 db entries with same activity and same hour
                    //
                    //DateTime date = Convert.ToDateTime(obj.Json.ScheduledAt);
                    //string cosmosDbFormat = date.ToString("yyyy-MM-ddTHH:mm:ss");
                    //string dateQuery = string.Format("SELECT COUNT(1) as count FROM c WHERE c.Json.Activity = '{0}' " +
                    //"AND c.User.Email != '{1}' AND DateTimeDiff(\"hh\", c.Json.ScheduledAt, '{2}') = 0", obj.Json.Activity, obj.User.Email, cosmosDbFormat);

                    // For debug purposes, only doing same activity for now
                    string dataQuery = string.Format("SELECT c.id, c.Json.Title, " +
                        "c.Json.Activity, c.Json.ScheduledAt, c.User.Username, c.User.Email FROM c WHERE c.Json.Activity = @Activity AND c.User.Email != @Email AND c.Status = 'unassigned' AND c.Json.Title = @Title", obj.Json.Activity, obj.User.Email, obj.Json.Title);
                    QueryDefinition queryDefinition = new QueryDefinition(dataQuery)
                                            .WithParameter("@Activity", obj.Json.Activity)
                                            .WithParameter("@Email", obj.User.Email)
                                            .WithParameter("@Title", obj.Json.Title);
                        
                    var query = container.GetItemQueryIterator<dynamic>(queryDefinition);
                    
                    List<string> primaryKeyList = new List<string>();
                    while (query.HasMoreResults)
                    {
                        FeedResponse<dynamic> result = await query.ReadNextAsync();
                        // Are there 2 people other than you with the same activity?
                        if (result.Count >= 2)
                        {
                            string mainKey = Convert.ToString(obj.id);
                            primaryKeyList.Add(mainKey);
                            foreach (var item in result)
                            {
                                primaryKeyList.Add((string)item.id);
                                // Send RabbitMQ message to Activity servcice to create activity and delete from AgendaServiceDB
                            }
                            _rabbit.SendMessage(primaryKeyList, "new_activity", new { foo = "bar" });
                            Console.WriteLine(primaryKeyList);
                        }
                    }
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

            dynamic dbEntry = await container.CreateItemAsync<dynamic>(
                item: obj,
                partitionKey: new PartitionKey(guid.ToString()));


            //_repo.AddAsync(item);
        }
    }
}
