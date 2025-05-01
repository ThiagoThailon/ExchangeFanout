using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


class Subscriber2
{
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri("amqps://qycjilrn:TFvVbNHx0l5B29sHVg9PBY6KyqePfPlT@fuji.lmq.cloudamqp.com/qycjilrn")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        string queueName = "despachoQueue";
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: queueName, exchange: "pedidoExchange", routingKey: "");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine($"Pedido para despacho recebido: {message}");

            try
            {
                // Desserializando corretamente para a classe Pedido
                var pedido = JsonSerializer.Deserialize<Pedido>(message);

                if (pedido == null || pedido.Id <= 0)
                {
                    Console.WriteLine("Erro: Pedido inválido.");
                    return;
                }

                // Construindo o objeto atualizado
                var pedidoAtualizado = new Pedido
                {
                    Id = pedido.Id,
                    Cliente = pedido.Cliente,
                    Produto = pedido.Produto,
                    Despachado = true // Apenas atualizando o status
                };

                using var httpClient = new HttpClient();
                var content = new StringContent(JsonSerializer.Serialize(pedidoAtualizado), Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"https://localhost:7069/api/Pedidos/{pedido.Id}", content);

                Console.WriteLine(response.IsSuccessStatusCode ? "Pedido despachado com sucesso!" : $"Erro ao despachar pedido: {response.StatusCode}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao desserializar JSON: {ex.Message}");
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        Console.ReadLine();
    }
}

// Classe Pedido para garantir a correta desserialização
class Pedido
{
    public int Id { get; set; }
    public string Cliente { get; set; }
    public string Produto { get; set; }
    public bool Despachado { get; set; }
} 