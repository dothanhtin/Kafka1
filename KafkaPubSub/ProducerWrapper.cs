using Confluent.Kafka;
using Shared.Constants;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KafkaPubSub
{
    public class ProducerWrapper
    {
        private string _topicName;
        private string _key;
        private ProducerBuilder<string, string> _producer;
        private ProducerConfig _config;
        //private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config, string topicName, string key)
        {
            this._topicName = topicName;
            this._config = config;
            this._key = key;
            this._producer = new ProducerBuilder<string, string>(this._config);
        }
        public async Task<string> writeMessage(string message)
        {
            using (var iProducer = this._producer.Build())
            {
                //var key = Guid.NewGuid();
                try
                {
                    var dr = await iProducer.ProduceAsync(this._topicName, new Message<string, string>()
                    {
                        //Key = key.ToString(),
                        Key = _key,
                        Value = message
                    });
                    Debug.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<string, string> e)
                {
                    Debug.WriteLine($"Failed to deliver message: {e.Error.Reason}");
                    //set status = 0
                    KafkaConfig.Instance.status = KafkaHelper.statusOffKafka;
                    //key = Guid.Empty;
                    _key = string.Empty;
                }
                iProducer.Flush(TimeSpan.FromSeconds(60.0));
                //return _key;
                return _key;
            }
        }
    }
}
