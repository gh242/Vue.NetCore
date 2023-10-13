//using Confluent.Kafka;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using VOL.Core.Configuration;
//using VOL.Core.Enums;
//using VOL.Core.KafkaManager.IService;
//using VOL.Core.Services;

//namespace VOL.Core.KafkaManager.Service
//{
//    /// <summary>
//    /// 消費者 (Message.Key的數據類型為string、Message.Value的數據類型為string）
//    /// 消費者實現三種消費方式：1.訂閱回調模式 2.批量消費模式 3.單筆消費模式
//    /// </summary>
//    /// <typeparam name="TKey">Message.Key 的數據類型</typeparam>
//    /// <typeparam name="TValue">Message.Value 的數據類型</typeparam>
//    public class KafkaConsumer<TKey, TValue> : KafkaConfig, IKafkaConsumer<TKey, TValue>
//    {
//        /// <summary>
//        ///  Kafka地址(包含端口號)
//        /// </summary>
//        public string Servers
//        {
//            get
//            {
//                return ConsumerConfig.BootstrapServers;
//            }
//            set
//            {
//                ConsumerConfig.BootstrapServers = value;
//            }
//        }

//        /// <summary>
//        /// 消費者群組
//        /// </summary>
//        public string GroupId
//        {
//            get
//            {
//                return ConsumerConfig.GroupId;
//            }
//            set
//            {
//                ConsumerConfig.GroupId = value;
//            }
//        }

//        /// <summary>
//        /// 自動提交 默認為 false
//        /// </summary>
//        public bool EnableAutoCommit
//        {
//            get
//            {
//                return ConsumerConfig.EnableAutoCommit ?? false;
//            }
//            set
//            {
//                ConsumerConfig.EnableAutoCommit = value;
//            }
//        }

//        /// <summary>
//        /// 訂閱回調模式-消費(持續訂閱)
//        /// </summary>
//        /// <param name="Func">回調函數,若配置為非自動提交(默認為否),則通過回調函數的返回值判斷是否提交</param>
//        /// <param name="Topic">主題</param>
//        public void Consume(Func<ConsumeResult<TKey, TValue>, bool> Func, string Topic)
//        {
//            Task.Factory.StartNew(() =>
//            {
//                var builder = new ConsumerBuilder<TKey, TValue>(ConsumerConfig);
//                //設置反序列化方式
//                builder.SetValueDeserializer(new KafkaDConverter<TValue>());
//                builder.SetErrorHandler((_, e) =>
//                {
//                    Logger.Error(LoggerType.KafkaException, null, null, $"Error:{e.Reason}");
//                }).SetStatisticsHandler((_, json) =>
//                {
//                    Console.WriteLine($"-{DateTime.Now:yyyy-MM-dd HH:mm:ss} > 消息監聽中..");
//                }).SetPartitionsAssignedHandler((c, partitions) =>
//                {
//                    string partitionsStr = string.Join(", ", partitions);
//                    Console.WriteLine($"-分配的kafka分區:{partitionsStr}");
//                }).SetPartitionsRevokedHandler((c, partitions) =>
//                {
//                    string partitionsStr = string.Join(", ", partitions);
//                    Console.WriteLine($"-回收了kafka的分區:{partitionsStr}");
//                });
//                using var consumer = builder.Build();
//                consumer.Subscribe(Topic);
//                while (AppSetting.Kafka.IsConsumerSubscribe) //true
//                {
//                    ConsumeResult<TKey, TValue> result = null;
//                    try
//                    {
//                        result = consumer.Consume();
//                        if (result.IsPartitionEOF) continue;
//                        if (Func(result))
//                        {
//                            if (!(bool)ConsumerConfig.EnableAutoCommit)
//                            {
//                                //手動提交，如果上面的EnableAutoCommit=true表示自動提交，則無需調用Commit方法
//                                consumer.Commit(result);
//                            }
//                        }
//                    }
//                    catch (ConsumeException ex)
//                    {
//                        Logger.Error(LoggerType.KafkaException, $"Topic:{Topic},{ex.Error.Reason}", null, ex.Message + ex.StackTrace);
//                    }
//                    catch (Exception ex)
//                    {
//                        Logger.Error(LoggerType.KafkaException, $"Topic:{result.Topic}", null, ex.Message + ex.StackTrace);
//                    }
//                }
//            });
//        }

//        /// <summary>
//        /// 批量訂閱回調模式-消費(持續訂閱)
//        /// </summary>
//        /// <param name="Func">回調函數,若配置為非自動提交(默認為否),則通過回調函數的返回值判斷是否提交</param>
//        /// <param name="Topic">主題</param>
//        public void ConsumeBatch(Func<ConsumeResult<TKey, TValue>, bool> Func, List<string> Topics)
//        {
//            Task.Factory.StartNew(() =>
//            {
//                var builder = new ConsumerBuilder<TKey, TValue>(ConsumerConfig);
//                //設置反序列化方式
//                builder.SetValueDeserializer(new KafkaDConverter<TValue>());
//                builder.SetErrorHandler((_, e) =>
//                {
//                    Logger.Error(LoggerType.KafkaException, null, null, $"Error:{e.Reason}");
//                }).SetStatisticsHandler((_, json) =>
//                {
//                    Console.WriteLine($"-{DateTime.Now:yyyy-MM-dd HH:mm:ss} > 消息監聽中..");
//                }).SetPartitionsAssignedHandler((c, partitions) =>
//                {
//                    string partitionsStr = string.Join(", ", partitions);
//                    Console.WriteLine($"-分配的kafka分區:{partitionsStr}");
//                }).SetPartitionsRevokedHandler((c, partitions) =>
//                {
//                    string partitionsStr = string.Join(", ", partitions);
//                    Console.WriteLine($"-回收了kafka的分區:{partitionsStr}");
//                });
//                using var consumer = builder.Build();
//                consumer.Subscribe(Topics);
//                while (AppSetting.Kafka.IsConsumerSubscribe) //true
//                {
//                    ConsumeResult<TKey, TValue> result = null;
//                    try
//                    {
//                        result = consumer.Consume();
//                        if (result.IsPartitionEOF) continue;
//                        if (Func(result))
//                        {
//                            if (!(bool)ConsumerConfig.EnableAutoCommit)
//                            {
//                                //手動提交，如果上面的EnableAutoCommit=true表示自動提交，則無需調用Commit方法
//                                consumer.Commit(result);
//                            }
//                        }
//                    }
//                    catch (ConsumeException ex)
//                    {
//                        Logger.Error(LoggerType.KafkaException, $"Topic:{Topics.ToArray()},{ex.Error.Reason}", null, ex.Message + ex.StackTrace);
//                    }
//                    catch (Exception ex)
//                    {
//                        Logger.Error(LoggerType.KafkaException, $"Topic:{result.Topic}", null, ex.Message + ex.StackTrace);
//                    }
//                }
//            });
//        }

//        /// <summary>
//        /// 批量消費模式-單次消費(消費出當前Kafka緩存的所有數據,並持續監聽 300ms,如無新數據生產,則返回(最多一次消費 100條)
//        /// </summary>
//        /// <param name="Topic">主題</param>
//        /// <param name="TimeOut">持續監聽時間,單位ms 默認值:300ms</param>
//        /// <param name="MaxRow">最多單次消費行數 默認值:100行</param>
//        /// <returns>待消費數據</returns>
//        public List<ConsumeResult<TKey, TValue>> ConsumeOnce(string Topic, int TimeOut = 300, int MaxRow = 100)
//        {
//            var builder = new ConsumerBuilder<TKey, TValue>(ConsumerConfig);
//            //設置反序列化方式
//            builder.SetValueDeserializer(new KafkaDConverter<TValue>());
//            using var consumer = builder.Build();
//            consumer.Subscribe(Topic);
//            List<ConsumeResult<TKey, TValue>> Res = new List<ConsumeResult<TKey, TValue>>();
//            while (true)
//            {
//                try
//                {
//                    var result = consumer.Consume(TimeSpan.FromMilliseconds(TimeOut));
//                    if (result == null) break;
//                    else
//                    {
//                        Res.Add(result);
//                        //手動提交，如果上面的EnableAutoCommit=true表示自動提交，則無需調用Commit方法
//                        consumer.Commit();
//                    }
//                    if (Res.Count > MaxRow) break;
//                }
//                catch (Exception ex)
//                {
//                    Logger.Error(LoggerType.KafkaException, $"Topic:{Topic}", null, ex.Message + ex.StackTrace);
//                    return null;
//                }
//            }
//            return Res;
//        }

//        /// <summary>
//        /// 單筆消費模式-單行消費
//        /// </summary>
//        /// <param name="Topic">主題</param>
//        /// <param name="TimeOut">持續監聽時間,單位ms 默認值:300ms</param>
//        /// <returns>待消費數據</returns>
//        public ConsumeResult<TKey, TValue> ConsumeOneRow(string Topic, int TimeOut = 300)
//        {
//            var builder = new ConsumerBuilder<TKey, TValue>(ConsumerConfig);
//            //設置反序列化方式
//            builder.SetValueDeserializer(new KafkaDConverter<TValue>());
//            using var consumer = builder.Build();
//            consumer.Subscribe(Topic);
//            try
//            {
//                var result = consumer.Consume(TimeSpan.FromMilliseconds(TimeOut));
//                if (result != null)
//                {
//                    //手動提交，如果上面的EnableAutoCommit=true表示自動提交，則無需調用Commit方法
//                    consumer.Commit();
//                }
//                return result;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error(LoggerType.KafkaException, $"Topic:{Topic}", null, ex.Message + ex.StackTrace);
//                return null;
//            }
//        }

//        public void Dispose()
//        {
//            //if (_cache != null)
//            //    _cache.Dispose();
//            GC.SuppressFinalize(this);
//        }
//    }
//}
