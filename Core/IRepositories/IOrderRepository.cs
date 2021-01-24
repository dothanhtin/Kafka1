using Kafka1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IOrderRepository
    {
        Task<int> createTestValue(OrderRequest orderRequest);
    }
}
