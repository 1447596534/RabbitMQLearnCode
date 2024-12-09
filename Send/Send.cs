// See https://aka.ms/new-console-template for more information


using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");
decimal.TryParse("asd", out decimal parameterxf1);


var factory = new ConnectionFactory { HostName ="localhost" };
var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "task_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);


string message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
Console.WriteLine($" [x] Send {message}");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}
