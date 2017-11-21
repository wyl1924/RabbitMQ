using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class RabbitMQManager
    {
        private static readonly string _serverAddress;
        private static readonly string _virtualHost;
        private static readonly string _userName;
        private static readonly string _password;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMQManager));
        private static RabbitProducer _rabbitProducer;

        static RabbitMQManager()
        {
            _serverAddress = ConfigurationManager.AppSettings["serveraddress"];
            _virtualHost = ConfigurationManager.AppSettings["virtualhost"];
            _userName = ConfigurationManager.AppSettings["username"];
            _password = ConfigurationManager.AppSettings["password"];

        }

        /// <summary>
        /// 交换链接信息
        /// </summary>
        /// <param name="routingKey">路由关键字</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息内容</param>
        public static void SendRabbitMQ(string routingKey, string queueName, string message)
        {
            RabbitProducerConfig _rabbitConfig = new RabbitProducerConfig()
            {
                ServerAddress = _serverAddress,
                VirtualHost = _virtualHost,
                UserName = _userName,
                Password = _password,
                Exchange = "erp.service",
                ExchangeType = "direct",
                RoutingKey = routingKey
            };
            if (_rabbitProducer == null || !_rabbitProducer.IsOpen)
            {
                _rabbitProducer = new RabbitProducer(_rabbitConfig);
            }
            try
            {
                _rabbitProducer.ProduceMessage(message, queueName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                _rabbitProducer.Close();
            }
        }

      
    }
}
