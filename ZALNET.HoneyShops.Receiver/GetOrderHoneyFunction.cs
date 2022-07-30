using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ZALNET.HoneyShops.Receiver
{
    public class GetOrderHoneyFunction
    {
        [FunctionName("GetOrderHoneyFunction")]
        public void Run([ServiceBusTrigger("orders", Connection = "ConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
