using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace AllDailyDuties_AgendaService.Middleware.Messaging
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public void SendMessage<T>(T message, string queue, object obj)
        {
            using var channel = RabbitMQConnection.Instance.Connection.CreateModel();
            channel.QueueDeclare(queue, exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            var props = channel.CreateBasicProperties();
            string correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;

            channel.BasicPublish(exchange: "", routingKey: queue, props, body: body);
            //Convert anonymous object to JSON
            var jsonObj = JsonConvert.SerializeObject(obj);

            var cache = RedisConnection.Connection.GetDatabase();
            //Store object & corrId in Redis
            cache.StringSet(correlationId, jsonObj);
        }

    }
}
