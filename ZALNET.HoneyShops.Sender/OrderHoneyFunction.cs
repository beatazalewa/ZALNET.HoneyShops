using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
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
            QueueClient client = new QueueClient("", "orders");

            string name = req.Query["name"];
            string order = req.Query["order"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            order = order ?? data?.order;

            string responseMessage;

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(order))
            {
                responseMessage = "This HTTP triggered function executed successfully. Pass a name and an order in the query string or in the request body for a personalized response.";
            }
            else
            {
                responseMessage = $"Hello, {name}. Your order is: {order}.";

                Message message = new Message(Encoding.UTF8.GetBytes(responseMessage));
                await client.SendAsync(message);
            }
            return new OkObjectResult(responseMessage);
        }
    }
}
