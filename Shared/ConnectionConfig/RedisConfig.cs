using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ConnectionConfig
{
    public class RedisConfig
    {
        public RedisConfiguration redisConfiguration { get; set; }
        public static RedisConfig Instance { get; } = new RedisConfig();
    }
}
