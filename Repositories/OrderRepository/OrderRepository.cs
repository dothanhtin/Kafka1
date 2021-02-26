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
            _db = MongoDBConnection._client.GetDatabase("testdb1");
        }
        public async Task<int> createTestValue(OrderRequest orderRequest)
        {
            try
            {
                var col = _db.GetCollection<BsonDocument>("testModelCollection");
                var doc = orderRequest.getBsonObject();
                await col.InsertOneAsync(doc);

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
