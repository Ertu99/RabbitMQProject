using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQProject.RabbitMQ
{
    public class RabitMQProducer : IRabitMQProducer
    {
        public void SendProductMessage<T>(T message)
        {
            //Burada Rabbit MQ Sunucusunu belirtiyoruz. rabbitmq docker imajını kullanıyoruz
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
            };

            //rabbitmq baglantisini olusturma
            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("product",exclusive:false);

            //mesaji serialize etme
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            //datalari kuyruga yerlestirme
            channel.BasicPublish(exchange: "", routingKey: "product", body: body);

        }


    }
}
