using AuthenticationRepository.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ListenForIntegrationEvents();
            CreateHostBuilder(args).Build().Run();
        }
        private static void ListenForIntegrationEvents()
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => {
                var contextOptions = new DbContextOptionsBuilder<LoginDBContext>().UseSqlServer(@"data source=(localdb)\MSSQLLocalDB; database=LoginDB; integrated security=true").Options;
                var dbContext = new LoginDBContext(contextOptions);
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
                if (type == "customer.add")
                {
                    dbContext.Customers.Add(new Customer() { CustomerId = data["CustomerId"].Value<int>(), Email = data["Email"].Value<string>() });
                    dbContext.SaveChanges();
                }
               
            };
            channel.BasicConsume(queue: "customer.postservice", autoAck: true, consumer: consumer);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
