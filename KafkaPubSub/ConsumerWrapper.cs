using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KafkaPubSub
{
    public class ConsumerWrapper
    {
        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private ConsumerBuilder<string, string> _consumer;
        private static readonly Random rand = new Random();
        public ConsumerWrapper(ConsumerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._consumerConfig = config;
            this._consumer = new ConsumerBuilder<string, string>(this._consumerConfig);
        }

        [Obsolete]
        public string readMessage()
        {
            using (var iConsumer = this._consumer.Build())
            {
                iConsumer.Subscribe(_topicName);
                var consumeResult = iConsumer.Consume();
                return consumeResult.Value;
            }
        }
    }
}
