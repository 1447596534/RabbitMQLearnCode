// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, RabbitMQ!");


var factory = new ConnectionFactory { HostName = "localhost" };
var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false); // qos调度，当前通道队列每个消费者同时最多只分配1个任务

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"[X] Received {message}");

    int dots = message.Split('.').Length - 1;
    await Task.Delay(dots * 1000);

    Console.WriteLine($"[X] Done");

    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);  // 确保当前在没处理完时出现中止会自动转交给别的消费者
};

await channel.BasicConsumeAsync("task_queue", autoAck: false, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();