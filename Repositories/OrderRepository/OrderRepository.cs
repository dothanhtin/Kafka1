using Core.IRepositories;
using Kafka1.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Connections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OrderRepository
{
    public class OrderRepository: IOrderRepository
    {
        private static IMongoDatabase _db;
        public OrderRepository()
        {
            _db = MongoDBConnection._client.GetDatabase("testdb");
        }
        public async Task<int> createTestValue(OrderRequest orderRequest)
        {
            try
            {
                var col = _db.GetCollection<BsonDocument>("testModelCollection");
                await col.InsertOneAsync(orderRequest.getBsonObject());

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
