using Core.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka1.Models
{
    public class OrderRequest : BaseAuditModel
    {
        public string clientId { get; set; }

        public BsonDocument getBsonObject()
        {
            return new BsonDocument
            {
                {nameof(clientId),clientId },
                {nameof(createdOn),createdOn },
                {nameof(createdBy),createdBy },
                {nameof(updatedOn),updatedOn },
                {nameof(updatedBy),updatedOn }
            };
        }

        public override string objectCollection()
        {
            throw new NotImplementedException();
        }
    }
}
