using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public abstract class BaseAuditModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string createdBy { get; set; }
        [BsonDateTimeOptions]
        public DateTime createdOn { get; set; }
        public string updatedBy { get; set; }
        [BsonDateTimeOptions]
        public DateTime updatedOn { get; set; }
        public abstract string objectCollection();
    }
}
