using Core.IRepositories;
using Kafka1.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Shared.Connections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private static IMongoDatabase _db;
        public OrderRepository()
        {
            _db = MongoDBConnection._client.GetDatabase("testdb1");
        }
        /// <summary>
        /// create order
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public async Task<string> createOrder(OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            try
            {
                var col = _db.GetCollection<BsonDocument>("testModelCollection");
                var doc = orderRequest.getBsonObject();
                await col.InsertOneAsync(doc);

                return orderRequest.id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// update order
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public async Task<string> updateOrder(OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            try
            {
                var col = _db.GetCollection<BsonDocument>("testModelCollection");
                var doc = orderRequest.getBsonObject();
                var filter = Builders<BsonDocument>.Filter.Eq("id", orderRequest.id);
                await col.UpdateOneAsync(filter, doc);
                return orderRequest.id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Delete order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> deleteOrder(string id, CancellationToken cancellationToken)
        {
            try
            {
                var col = _db.GetCollection<BsonDocument>("testModelCollection");
                var filter = Builders<BsonDocument>.Filter.Eq("id", id);
                await col.DeleteOneAsync(filter);
                return id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderRequest> getOrderById(string id, CancellationToken cancellationToken)
        {
            try
            {
                var col = _db.GetCollection<BsonDocument>("testModelCollection");
                var filter = Builders<BsonDocument>.Filter.Eq("id", id);
                var value = await col.FindAsync(filter);
                var res = BsonSerializer.Deserialize<OrderRequest>(value.ToBsonDocument());
                //test
                // return (OrderRequest)value;
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<OrderRequest>> getAllOrder(CancellationToken cancellationToken)
        {
            try
            {
                var col = _db.GetCollection<BsonDocument>("testModelCollection");
                var value = await col.FindAsync(new BsonDocument());
                var res = BsonSerializer.Deserialize<List<OrderRequest>>(value.ToBsonDocument());
                return res;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
