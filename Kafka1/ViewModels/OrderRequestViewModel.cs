using Kafka1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka1.ViewModels
{
    public class OrderRequestViewModel
    {
        public string orderId { get; set; }
        public string clientId { get; set; }
        public OrderRequest getObject()
        {
            return new OrderRequest
            {
                orderId = this.orderId,
                clientId = this.clientId,
                id = Guid.NewGuid().ToString(),
                createdOn = DateTime.Now,
                createdBy = clientId,
                updatedOn = DateTime.Now,
                updatedBy = clientId
            };
        }
    }
}
