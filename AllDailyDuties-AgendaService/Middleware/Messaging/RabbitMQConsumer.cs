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
        private IMessageService _message;

        public RabbitMQConsumer(IMessageService message)
        {
            _message = message;
        }
        public void ConsumeMessage<T>(IModel channel, string queue)
        {
            //Debug console log
            Console.WriteLine("foo");
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

                    var output = await cache.StringGetAsync(props.CorrelationId);
                    await _message.CreateObject<T>(message, output, queue);
                    System.Threading.Thread.Sleep(50);

                }
            };
            //read the message
            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }
    }
}
