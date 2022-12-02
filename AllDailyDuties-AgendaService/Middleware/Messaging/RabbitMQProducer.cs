using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public string SendMessage<T>(T message, string queue)
        {
            using var channel = RabbitMQConnection.Instance.Connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare(queue, exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            var props = channel.CreateBasicProperties();
            string correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;

            channel.BasicPublish(exchange: "", routingKey: queue, props, body: body);

            var jsonObj = JsonConvert.SerializeObject(new { title = "my_post", description = "Such an awesome post!" });

            var cache = RedisConnection.Connection.GetDatabase();

            cache.StringSet(correlationId, jsonObj);


            return correlationId;
        }

    }
}
