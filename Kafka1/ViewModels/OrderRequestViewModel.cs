using Kafka1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka1.ViewModels
{
    public class OrderRequestViewModel
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public OrderRequest getObject()
        {
            if (string.IsNullOrEmpty(id))
                id = Guid.NewGuid().ToString();
            return new OrderRequest
            {
                clientId = this.clientId,
                id = this.id,
                createdOn = DateTime.Now,
                createdBy = clientId,
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
