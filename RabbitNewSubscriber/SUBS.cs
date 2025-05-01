using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
/*
class Subscriber1
{
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri("amqps://qycjilrn:TFvVbNHx0l5B29sHVg9PBY6KyqePfPlT@fuji.lmq.cloudamqp.com/qycjilrn")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        string queueName = "pedidosQueue";
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: queueName, exchange: "pedidoExchange", routingKey: "");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine($"Pedido recebido: {message}");

            using var httpClient = new HttpClient();
            var content = new StringContent(message, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://localhost:7069/api/Pedidos", content);
            Console.WriteLine(response.IsSuccessStatusCode ? "Pedido registrado com sucesso!" : $"Erro ao registrar pedido: {response.StatusCode}");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        Console.ReadLine();
    }
} */