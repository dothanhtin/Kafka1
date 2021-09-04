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
            var now = DateTime.Now;
            if (string.IsNullOrEmpty(orderId))
                orderId = Guid.NewGuid().ToString();
            return new OrderRequest
            {
                orderId = orderId,
                clientId = this.clientId,
                _id = this.orderId,
                createdOn = now,
                createdBy = clientId,
                updatedOn = now,
                updatedBy = clientId
            };
        }
        public OrderRequest getUpdateObject()
        {
            return new OrderRequest
            {
                orderId = orderId,
                clientId = this.clientId,
                updatedOn = DateTime.Now,
                updatedBy = clientId
            };
        }
    }
    public class DeleteViewModel
    {
        public string id { get; set; }
    }
}
