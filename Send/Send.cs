// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, RabbitMQ（NewTask/Producer）!");

var factory = new ConnectionFactory { HostName ="localhost" };
var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
//args = new string[] { "tenth message.........." };
string message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

var properties = new BasicProperties()
{
    Persistent = true,
}; // 持久化消息，队列生成后，RabbitMQ服务停止队列也不会丢失

// 如果发布消息时使用的是持久化队列，即服务停止也会将消息发布到队列，服务恢复后消息会开始消费
await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "task_queue", mandatory: true, basicProperties: properties, body: body);
Console.WriteLine($" [x] Send {message}");

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}
