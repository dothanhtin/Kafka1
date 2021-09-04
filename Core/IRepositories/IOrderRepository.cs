using Kafka1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IOrderRepository
    {
        Task<string> createOrder(OrderRequest orderRequest, CancellationToken cancellationToken);
        Task<string> updateOrder(OrderRequest orderRequest, CancellationToken cancellationToken);
        Task<string> deleteOrder(string id, CancellationToken cancellationToken);
        Task<OrderRequest> getOrderById(string id, CancellationToken cancellationToken);
        Task<List<OrderRequest>> getAllOrder(CancellationToken cancellationToken);
    }
}
