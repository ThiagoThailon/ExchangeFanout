using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

class Publisher
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri("amqps://qycjilrn:TFvVbNHx0l5B29sHVg9PBY6KyqePfPlT@fuji.lmq.cloudamqp.com/qycjilrn")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        string exchangeName = "pedidoExchange";
        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

        // Criação dos objetos Pedido
        var addPedido = new
        {
            // Adicionando um ID único
            Cliente = "dd",
            Produto = "pc ga",
            Despachado = false
        };

        var despacharPedido = new
        {
            Id = 14,
            Cliente = "dd",
            Produto = "pc ga",
            Despachado = true
        };

        // Serialização e publicação das mensagens
        PublishMessage(channel, exchangeName, addPedido);
        //
        PublishMessage(channel, exchangeName, despacharPedido);

        Console.WriteLine($"Pedidos enviados para o Exchange.");
    }

    static void PublishMessage(IModel channel, string exchange, object message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);
    }
}