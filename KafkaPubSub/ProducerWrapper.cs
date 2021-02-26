using Confluent.Kafka;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KafkaPubSub
{
    public class ProducerWrapper
    {
        private string _topicName;
        private ProducerBuilder<string, string> _producer;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._config = config;
            this._producer = new ProducerBuilder<string, string>(this._config);
        }
        public async Task writeMessage(string message)
        {
            using (var iProducer = this._producer.Build())
            {
                //var dr = await iProducer.ProduceAsync(this._topicName, new Message<string, string>()
                //{
                //    Key = rand.Next(5).ToString(),
                //    Value = message
                //});
                //Console.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                try
                {
                    var dr = await iProducer.ProduceAsync(this._topicName, new Message<string, string>()
                    {
                        Key = rand.Next(5).ToString(),
                        Value = message
                    });
                    Debug.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<string, string> e)
                {
                    Debug.WriteLine($"Failed to deliver message: {e.Error.Reason}");
                }
                iProducer.Flush(TimeSpan.FromSeconds(60.0));
                return;
            }
        }
    }
}
