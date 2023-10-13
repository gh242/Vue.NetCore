////using Confluent.Kafka;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using VOL.Core.Configuration;

//namespace VOL.Core.KafkaManager
//{
//    /// <summary>
//    /// 配置類
//    /// </summary>
//    public class KafkaConfig
//    {
//        /// <summary>
//        /// 構造配置類
//        /// </summary>
//        protected KafkaConfig()
//        {
//            //ProducerConfig = new ProducerConfig()
//            //{
//            //    BootstrapServers = AppSetting.Kafka.ProducerSettings.BootstrapServers,// "192.168.20.241:9092",
//            //};

//            //ConsumerConfig = new ConsumerConfig()
//            //{
//            //    BootstrapServers = AppSetting.Kafka.ConsumerSettings.BootstrapServers,
//            //    GroupId = AppSetting.Kafka.ConsumerSettings.GroupId,
//            //    AutoOffsetReset = AutoOffsetReset.Earliest,//當各分區下有已提交的offset時，從提交的offset開始消費；無提交的offset時，從頭開始消費
//            //    EnableAutoCommit = false,
//            //    //Kafka配置安全認證
//            //    //SecurityProtocol = SecurityProtocol.SaslPlaintext,
//            //    //SaslMechanism = SaslMechanism.Plain,
//            //    //SaslUsername = AppSetting.Kafka.ConsumerSettings.SaslUsername,
//            //    //SaslPassword = AppSetting.Kafka.ConsumerSettings.SaslPassword,
//            //};
//        }

//        ///// <summary>
//        ///// 消費者配置文件
//        ///// </summary>
//        //public ConsumerConfig ConsumerConfig;

//        ///// <summary>
//        ///// 生產者配置文件
//        ///// </summary>
//        //public ProducerConfig ProducerConfig;
//    }
//}