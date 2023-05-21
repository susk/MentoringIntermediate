using System;
using RabbitMQ.Client;
using MessagingService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Subscriber
{
    public class Subscriber
    {
        public static void Main(string[] args)
        {
            using (IConnection connection = GetConnectionConfiguration())
            {
                using IModel model = connection.CreateModel();
                Console.WriteLine(" [*] Waiting for logs.");

                string exchangeType = GetExchangeTypeConfiguration();

                ReceiveMessages(model, exchangeType);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        private static IConnection GetConnectionConfiguration()
        {
            using IHost host = Host.CreateDefaultBuilder().Build();

            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

            string hostName = config.GetSection("Connection")["HostName"] ?? "";
            string userName = config.GetSection("Connection")["User"] ?? "";
            string password = config.GetSection("Connection")["Password"] ?? "";

            if (hostName == "" || userName == "" || password == "")
            {
                throw new NullReferenceException("Could not establish connection due to wrong settings");
            }

            RabbitMqService rabbitMqService = new RabbitMqService(hostName, userName, password);
            
            return rabbitMqService.GetRabbitMqConnection;
        }
        private static string GetExchangeTypeConfiguration()
        {
            using IHost host = Host.CreateDefaultBuilder().Build();

            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

            return config.GetValue<string>("ExchangeType") ?? "";
        }
        private static void ReceiveMessages(IModel model, string exchangeType)
        {
            if (exchangeType == MessagingService.RabbitMqService.ExchangeType.LargeMessageBufferedQueue)
            {
                MessagingService.RabbitMqService.ReceiveBufferedMessages(model);
            }
            else if (exchangeType == MessagingService.RabbitMqService.ExchangeType.ChunkedMessageBufferedQueue)
            {
                MessagingService.RabbitMqService.ReceiveChunkedMessages(model);
            }
            else
            {
                throw new Exception("The exchange type is unknown: Please check the configuration type in the appsettings.json");
            }
        }
    }
}


