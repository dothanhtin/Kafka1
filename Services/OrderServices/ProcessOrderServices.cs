using Core.IRepositories;
using Core.IServices.IOrderServices;
using Kafka1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.OrderServices
{
    public class ProcessOrderServices : IProcessOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        public ProcessOrderServices(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<string> CreateOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            var res = await _orderRepository.createOrder(orderRequest, cancellationToken);
            return res;
        }
        public async Task<string> UpdateOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            var res = await _orderRepository.updateOrder(orderRequest, cancellationToken);
            return res;
        }
        public async Task<string> DeleteOrderAsync(string id, CancellationToken cancellationToken)
        {
            var res = await _orderRepository.deleteOrder(id, cancellationToken);
            return res;
        }
        public async Task<OrderRequest> GetOrderById(string id, CancellationToken cancellationToken)
        {
            var res = await _orderRepository.getOrderById(id, cancellationToken);
            return res;
        }
        public async Task<List<OrderRequest>> GetAllOrderAsync(CancellationToken cancellationToken)
        {
            var res = await _orderRepository.getAllOrder(cancellationToken);
            return res;
        }
    }
}
