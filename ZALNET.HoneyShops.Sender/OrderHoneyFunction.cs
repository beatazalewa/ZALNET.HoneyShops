using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace ZALNET.HoneyShops.Sender
{
    public static class OrderHoneyFunction
    {
        [FunctionName("OrderHoneyFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            // Create a ServiceBusClient that will authenticate using a connection string
            string connectionString = "Endpoint=sb://zalnethoneyshops.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=clWiyF1jrNFuioVhuFrfVxqKNQwRL/BI5kxGiswbJ5w=";
            string queueName = "orders";
            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            await using var client = new ServiceBusClient(connectionString);
            
            //QueueClient client = new QueueClient("", "orders");

            string name = req.Query["name"];
            string order = req.Query["order"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            order = order ?? data?.order;
            // create the sender
            ServiceBusSender sender = client.CreateSender(queueName);
            // create a message that we can send. UTF-8 encoding is used when providing a string.
            //ServiceBusMessage message = new ServiceBusMessage($"Hello, {name}. Your order is: {order}.");
            ServiceBusMessage message = new ServiceBusMessage($"Hello Beata");
            await sender.SendMessageAsync(message);
            return new OkObjectResult(message);
        }
    }
}
