using Kafka1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.IServices.IOrderServices
{
    public interface IProcessOrderServices
    {
        Task<string> CreateOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken);
        Task<string> UpdateOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken);
        Task<string> DeleteOrderAsync(string id, CancellationToken cancellationToken);
        Task<OrderRequest> GetOrderById(string id, CancellationToken cancellationToken);
        Task<List<OrderRequest>> GetAllOrderAsync(CancellationToken cancellationToken);
    }
}
