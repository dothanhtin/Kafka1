using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaPubSub
{
    public class KafkaConfig
    {
        public ProducerConfig producerConfig { get; set; }
        public ConsumerConfig consumerConfig { get; set; }
    }
}
