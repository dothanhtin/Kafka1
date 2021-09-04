using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Constants
{
    public class KafkaHelper
    {
        public const string orderRequestTopic = "orderrequests";
        public const int numOfConsumerWith_OrderRequestTopic = 5;
        public const int statusOffKafka = 0;
        public const int statusOnKafka = 1;
    }
}