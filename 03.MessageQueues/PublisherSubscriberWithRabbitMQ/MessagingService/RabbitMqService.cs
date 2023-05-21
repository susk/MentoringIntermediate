using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Configuration;
using System.Text;

namespace MessagingService
{
    public class RabbitMqService
    {
        private readonly ConnectionFactory connectionFactory;

        public static class ExchangeType
        {            
            public static string LargeMessageBufferedQueue { get { return "LargeMessageBufferedQueue"; } }
            public static string ChunkedMessageBufferedQueue { get { return "ChunkedMessageBufferedQueue"; } }
        }
       
        public IConnection GetRabbitMqConnection { get { return connectionFactory.CreateConnection(); } }

        public RabbitMqService(string hostName, string userName, string password)
        {
            connectionFactory = new()
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };
        }
        public static void ReceiveChunkedMessages(IModel model)
        {
            model.BasicQos(0, 1, false);
            // QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            var consumer = new EventingBasicConsumer(model);

            model.ExchangeDeclare(exchange: ExchangeType.ChunkedMessageBufferedQueue, "Fanout");
            var queueName = model.QueueDeclare(queue: ExchangeType.ChunkedMessageBufferedQueue).QueueName;
            model.QueueBind(queue: queueName, exchange: ExchangeType.ChunkedMessageBufferedQueue, routingKey: "");

            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("Received a chunk!");
                IDictionary<string, object> headers = ea.BasicProperties.Headers;
                string randomFileName = Encoding.UTF8.GetString((byte[])headers["output-file"]);
                bool isLastChunk = Convert.ToBoolean(headers["finished"]);
                string localFileName = string.Concat(@"..\..\..\..\Output\large_file_from_rabbit_", randomFileName, ".txt");

                using (FileStream fileStream = new FileStream(localFileName, FileMode.Append, FileAccess.Write))
                {
                    fileStream.Write(ea.Body.ToArray(), 0, ea.Body.Length);
                    fileStream.Flush();
                }
                Console.WriteLine("Chunk saved. Finished? {0}", isLastChunk);

            };

            model.BasicConsume(queue: ExchangeType.ChunkedMessageBufferedQueue, autoAck: false, consumer: consumer);
        }
        public static void ReceiveBufferedMessages(IModel model)
        {
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                string randomFileName = string.Concat(@"..\..\..\..\Output\large_file_from_rabbit_", Guid.NewGuid(), ".txt");
                File.WriteAllBytes(randomFileName, body);

                Console.WriteLine(" [x] {0}", message);
            };
            model.BasicConsume(queue: ExchangeType.LargeMessageBufferedQueue, autoAck: false, consumer: consumer);

        }

        public static void SendChunkedMessage(IModel model)
        {
            string filePath = @"~\..\..\..\..\..\Input\ForTest.txt";
            int chunkSize = 4096;

            Console.WriteLine("Starting file read operation...");
            FileStream fileStream = File.OpenRead(filePath);
            StreamReader streamReader = new StreamReader(fileStream);
            int remainingFileSize = Convert.ToInt32(fileStream.Length);
            _ = Convert.ToInt32(fileStream.Length);
            bool finished = false;
            string randomFileName = string.Concat("large_chunked_file_", Guid.NewGuid(), ".txt");
            byte[] buffer;
            while (true)
            {
                if (remainingFileSize <= 0)
                {
                    break;
                }
                int read;
                if (remainingFileSize > chunkSize)
                {
                    buffer = new byte[chunkSize];
                    read = fileStream.Read(buffer, 0, chunkSize);
                }
                else
                {
                    buffer = new byte[remainingFileSize];
                    read = fileStream.Read(buffer, 0, remainingFileSize);
                    finished = true;
                }

                IBasicProperties basicProperties = model.CreateBasicProperties();
                basicProperties.Persistent = true;
                basicProperties.Headers = new Dictionary<string, object>();
                basicProperties.Headers.Add("output-file", randomFileName);
                basicProperties.Headers.Add("finished", finished);

                model.BasicPublish("",RabbitMqService.ExchangeType.ChunkedMessageBufferedQueue, basicProperties, buffer);
                remainingFileSize -= read;
            }
            Console.WriteLine("Chunks complete.");
        }
        public static void SendBufferedMessage(IModel model)
        {
            string filePath = @"~\..\..\..\..\..\Input\ForTest.txt";

            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = true;
            byte[] fileContents = File.ReadAllBytes(filePath);

            model.BasicPublish("", RabbitMqService.ExchangeType.LargeMessageBufferedQueue, basicProperties, fileContents);
            Console.WriteLine("Sent: " + Encoding.UTF8.GetString(fileContents, 0, fileContents.Length));
        }

    }
}