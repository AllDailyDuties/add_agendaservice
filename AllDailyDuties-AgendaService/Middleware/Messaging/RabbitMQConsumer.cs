using AllDailyDuties_AgendaService.Helpers;
using AllDailyDuties_AgendaService.Middleware.Messaging.Interfaces;
using AllDailyDuties_AgendaService.Models.Shared;
using AllDailyDuties_AgendaService.Models.Tasks;
using AllDailyDuties_AgendaService.Repositories.Interfaces;
using AllDailyDuties_AgendaService.Services;
using AllDailyDuties_AgendaService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private ITaskService _taskService;
        private ITaskItemRepo _repo;
        private TaskUser user;
        private CreateRequest item;

        public RabbitMQConsumer(ITaskService taskService, ITaskItemRepo repo)
        {
            _taskService = taskService;
            _repo = repo;
        }
        public void ConsumeMessage(IModel channel, string queue)
        {
            var cache = RedisConnection.Connection.GetDatabase();
            

            channel.QueueDeclare(queue, exclusive: false);
            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var props = eventArgs.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                if(cache.KeyExists(props.CorrelationId)){
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    user = _taskService.GetTaskUser(message);

                    var output = cache.StringGet(props.CorrelationId);
                    TaskItemMessage taskItem = JsonConvert.DeserializeObject<TaskItemMessage>(output);
                    Guid guid = Guid.NewGuid();
                    item = new CreateRequest(guid, taskItem.Title, taskItem.CreatedAt, taskItem.ScheduledAt, user);
                }
                
                await _repo.AddAsync(item);

                //Console.WriteLine($"Token message received: {message} with corrId: {props.CorrelationId}");
            };
            //read the message
            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }
    }
}
