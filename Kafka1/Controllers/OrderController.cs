using Confluent.Kafka;
using Kafka1.ViewModels;
using KafkaPubSub;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ProducerConfig _producerConfig;
        public OrderController(ProducerConfig producerConfig)
        {
            this._producerConfig = producerConfig;
        }
        [HttpPost]
        public async Task<IActionResult> OrderRequest([FromBody] OrderRequestViewModel orderRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(orderRequest);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Recieved a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            var producer = new ProducerWrapper(this._producerConfig, "orderrequests");
            await producer.writeMessage(serializedOrder);

            //return Created("TransactionId", "Your order is in progress");
            return Ok("Your order is in progress");
        }
    }
}
