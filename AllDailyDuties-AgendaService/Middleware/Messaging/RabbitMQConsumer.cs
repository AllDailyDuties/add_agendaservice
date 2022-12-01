using AllDailyDuties_AgendaService.Middleware.Messaging.Interfaces;
using AllDailyDuties_AgendaService.Models.Shared;
using AllDailyDuties_AgendaService.Services;
using AllDailyDuties_AgendaService.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private ITaskService _taskService;
        public TaskUser user;
        public RabbitMQConsumer(ITaskService taskService)
        {
            _taskService = taskService;
        }
        public void ConsumeMessage(IModel channel, string queue)
        {
            channel.QueueDeclare(queue, exclusive: false);
            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var props = eventArgs.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                TaskUser user = _taskService.GetTaskUser(message);
                Console.WriteLine($"Token message received: {message}");
            };
            //read the message
            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }
    }
}
