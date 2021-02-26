using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
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
            this._consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            this._consumerConfig.AutoCommitIntervalMs = 3000;
            this._consumerConfig.AllowAutoCreateTopics = true;
            this._consumer = new ConsumerBuilder<string, string>(this._consumerConfig);
        }

        [Obsolete]
        public async Task<string> ReadMessage()
        {
            #region old code
            //try
            //{
            //    using (var iConsumer = this._consumer.Build())
            //    {
            //        iConsumer.Subscribe(_topicName);
            //        var consumeResult = iConsumer.Consume();
            //        if (consumeResult.Message != null)
            //            return consumeResult.Value;
            //        else
            //            return null;
            //    };
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
            #endregion

            #region new code
            return await Task.Run(() =>
            {
                //string result;
                using (var iConsumer = this._consumer.Build())
                {
                    iConsumer.Subscribe(_topicName);
                    //var consumeResult = iConsumer.Consume();
                    //return consumeResult.Value;
                    CancellationTokenSource cts = new CancellationTokenSource();
                    Console.CancelKeyPress += (_, e) =>
                    {
                        e.Cancel = true; // prevent the process from terminating.
                        cts.Cancel();
                    };

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var cr = iConsumer.Consume(cts.Token);
                                var mess = iConsumer.Consume(cts.Token).Message;
                                //Debug.WriteLine($"Consumed message '{cr.Value}' at: .");
                                if (mess != null)
                                {
                                    //result = cr.Value;
                                    Debug.WriteLine($"Consumed message '{mess.Value}' at: '{cr.TopicPartitionOffset}'.");
                                    _ = iConsumer.Commit();
                                    return mess.Value;
                                }
                                else
                                    //result = null;
                                    return null;
                            }
                            catch (ConsumeException e)
                            {
                                Debug.WriteLine($"Error occured: {e.Error.Reason}");
                                //result = null;
                                return null;
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Ensure the consumer leaves the group cleanly and final offsets are committed.
                        iConsumer.Close();
                        //result = null;
                        return null;
                    }
                }
                //return result;
            });
            #endregion
        }
    }
}
