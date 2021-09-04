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

                return orderRequest.orderId;
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
                #region using with replace
                //var doc = orderRequest.getBsonObject();
                //var filter = Builders<BsonDocument>.Filter.Eq("orderId", orderRequest.orderId);
                //await col.ReplaceOneAsync(filter, doc);
                #endregion
                #region update with update
                var filter = Builders<BsonDocument>.Filter.Eq("orderId", orderRequest.orderId);
                var update = Builders<BsonDocument>.Update.Set("clientId", orderRequest.clientId)
                                                           .Set("updatedBy", orderRequest.updatedBy)
                                                           .Set("updatedOn", orderRequest.updatedOn);
                await col.UpdateOneAsync(filter, update);
                #endregion
                return orderRequest.orderId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Delete order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<string> deleteOrder(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var col = _db.GetCollection<BsonDocument>("testModelCollection");
                var filter = Builders<BsonDocument>.Filter.Eq("orderId", orderId);
                await col.DeleteOneAsync(filter);
                return orderId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderRequest> getOrderById(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var col = _db.GetCollection<OrderRequest>("testModelCollection");
                //var filter = Builders<BsonDocument>.Filter.Eq("orderId", orderId);
                var value = await col.FindAsync(s => s.orderId == orderId);
                var res = value.FirstOrDefault();
                return res;
                //return BsonSerializer.Deserialize<OrderRequest>(res);
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
                var col = _db.GetCollection<OrderRequest>("testModelCollection");
                var value = await col.FindAsync(s => !string.IsNullOrEmpty(s.orderId));
                var res = value.ToList();
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
