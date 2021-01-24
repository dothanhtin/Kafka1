using MongoDB.Driver;
using Shared.ConnectionConfig;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Connections
{
    public class MongoDBConnection
    {
        //private readonly MongoDBConfiguration _mongoDBConfiguration;
        private static string _connectionString;
        public static MongoClient _client;
        public MongoDBConnection(MongoDBConfiguration mongoDBConfiguration)
        {
            //this._mongoDBConfiguration = mongoDBConfiguration;
            Connect(mongoDBConfiguration);
        }
        public void Connect(MongoDBConfiguration mongoDBConfiguration)
        {
            _connectionString = $"{MongoDBHelper.mongoPrefix}{mongoDBConfiguration.username}:{mongoDBConfiguration.password}@{mongoDBConfiguration.host}{MongoDBHelper.authSource}";
            _client = new MongoClient(_connectionString);
        }
    }
}
