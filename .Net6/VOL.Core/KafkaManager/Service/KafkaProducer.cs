//using Confluent.Kafka;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using VOL.Core.Enums;
//using VOL.Core.KafkaManager.IService;
//using VOL.Core.Services;

//namespace VOL.Core.KafkaManager.Service
//{
//    /// <summary>
//    /// 生產者 控制器或Service裡面構造函數注入即可調用
//    /// Message.Key的數據類型為string、Message.Value的數據類型為string
//    /// </summary>
//    /// <typeparam name="TKey">Message.Key 的數據類型</typeparam>
//    /// <typeparam name="TValue">Message.Value 的數據類型</typeparam>
//    public class KafkaProducer<TKey, TValue> : KafkaConfig, IKafkaProducer<TKey, TValue>
//    {
//        /// <summary>
//        /// 構造生產者
//        /// </summary>
//        public KafkaProducer()
//        {

//        }

//        /// <summary>
//        ///  Kafka地址(包含端口號)
//        /// </summary>
//        public string Servers
//        {
//            get
//            {
//                return ProducerConfig.BootstrapServers;
//            }
//            set
//            {
//                ProducerConfig.BootstrapServers = value;
//            }
//        }

//        /// <summary>
//        /// 生產
//        /// </summary>
//        /// <param name="Key">Message.Key 做消息指定分區投放有用的</param>
//        /// <param name="Value">Message.Value</param>
//        /// <param name="Topic">主題</param>
//        public void Produce(TKey Key, TValue Value, string Topic)
//        {
//            var producerBuilder = new ProducerBuilder<TKey, TValue>(ProducerConfig);
//            producerBuilder.SetValueSerializer(new KafkaConverter<TValue>());//設置序列化方式
//            using var producer = producerBuilder.Build();
//            try
//            {
//                producer.Produce(Topic, new Message<TKey, TValue>
//                {
//                    Key = Key,
//                    Value = Value
//                }, (result) =>
//                {
//                    if (result.Error.IsError)
//                        Logger.Error(LoggerType.KafkaException, $"Topic:{Topic},ServerIp:{KafkaHelper.GetServerIp()},ServerName:{ KafkaHelper.GetServerName()}", null, $"Delivery Error:{result.Error.Reason}");
//                });//Value = JsonConvert.SerializeObject(value)
//            }
//            catch (ProduceException<Null, string> ex)
//            {
//                Logger.Error(LoggerType.KafkaException, $"Topic:{Topic},Delivery failed: { ex.Error.Reason}", null, ex.Message + ex.StackTrace);
//            }
//        }

//        /// <summary>
//        /// 生產異步
//        /// </summary>
//        /// <param name="Key">Message.Key</param>
//        /// <param name="Value">Message.Value</param>
//        /// <param name="Topic">主題</param>
//        /// <returns></returns>
//        public async Task ProduceAsync(TKey Key, TValue Value, string Topic)
//        {
//            var producerBuilder = new ProducerBuilder<TKey, TValue>(ProducerConfig);
//            producerBuilder.SetValueSerializer(new KafkaConverter<TValue>());
//            using var producer = producerBuilder.Build();
//            try
//            {
//                var dr = await producer.ProduceAsync(Topic, new Message<TKey, TValue>
//                {
//                    Key = Key,
//                    Value = Value
//                });
//                //Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
//            }
//            catch (ProduceException<Null, string> ex)
//            {
//                Logger.Error(LoggerType.KafkaException, $"Topic:{Topic},ServerIp:{KafkaHelper.GetServerIp()},ServerName:{ KafkaHelper.GetServerName()},Delivery failed: { ex.Error.Reason}", null, ex.Message + ex.StackTrace);
//            }
//        }
//    }
//}
