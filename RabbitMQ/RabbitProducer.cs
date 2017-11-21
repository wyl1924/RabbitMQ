using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class RabbitProducer
    {
        private RabbitProducerConfig RBSendinfo;
        private ConnectionFactory cf = new ConnectionFactory();
        //是否对消息队列持久化保存
        bool persistMode = true;
        private IConnection conn; //建立联接
        public RabbitProducer(RabbitProducerConfig Rbinfo)
        {
            RBSendinfo = Rbinfo;
            cf = new ConnectionFactory()
            {
                UserName = RBSendinfo.UserName,
                Password = RBSendinfo.Password,
                VirtualHost = RBSendinfo.VirtualHost,
                RequestedHeartbeat = 0,
                Uri = RBSendinfo.ServerAddress
            };
            conn = cf.CreateConnection();
        }

        private void CheckConn()
        {
            if (RBSendinfo != null && !IsOpen)
            {
                cf = new ConnectionFactory()
                {
                    UserName = RBSendinfo.UserName,
                    Password = RBSendinfo.Password,
                    VirtualHost = RBSendinfo.VirtualHost,
                    RequestedHeartbeat = 0,
                    Uri = RBSendinfo.ServerAddress
                };
                conn = cf.CreateConnection();
            }
        }

        /// <summary>
        /// 初始化Rabbit连接，此方法只在测试时使用
        /// </summary>
        /// <param name="rbinfo"></param>
        public RabbitProducer(string username, string password, string virtualhost, string serveraddress, string exchange, string exchangetype, string routingkey)
        {
            cf = new ConnectionFactory()
            {
                UserName = username,
                Password = password,
                VirtualHost = virtualhost,
                RequestedHeartbeat = 0,
                Uri = serveraddress
            };
            conn = cf.CreateConnection();
            RBSendinfo = new RabbitProducerConfig()
            {
                UserName = username,
                Password = password,
                VirtualHost = virtualhost,
                ServerAddress = serveraddress,
                Exchange = exchange,
                ExchangeType = exchangetype,
                RoutingKey = routingkey
            };
        }

        public bool ProduceFanoutMessage(string body)
        {
            if (!RBSendinfo.ExchangeType.Equals("fanout"))
            {
                throw new RabbitException() { InternalException = new NotFanoutExchangeTypeException(), QueueName = "", RabbitInfo = RBSendinfo.ToString(), CurrentMessage = "" };
            }
            return ProduceMessage(body, "");
        }

        public bool ProduceFanoutMessage(string body, string queuename)
        {
            if (!RBSendinfo.ExchangeType.Equals("fanout"))
            {
                throw new RabbitException() { InternalException = new NotFanoutExchangeTypeException(), QueueName = queuename, RabbitInfo = RBSendinfo.ToString(), CurrentMessage = "" };
            }
            return ProduceMessage(body, queuename);
        }


        /// <summary>
        /// 插入队列
        /// </summary>
        /// <param name="body">消息内容</param>
        /// <param name="queuename">队列名称</param>
        public bool ProduceMessage(string body, string queuename)
        {

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentNullException("body");
            }
            CheckConn();
            //创建并返回一个新连接到具体节点的通道
            using (IModel ch = conn.CreateModel())
            {
                try
                {
                    if (RBSendinfo.ExchangeType != null)
                    {   //声明一个路由
                        ch.ExchangeDeclare(RBSendinfo.Exchange, RBSendinfo.ExchangeType);
                        //声明一个队列
                        if (!string.IsNullOrEmpty(queuename))
                        {
                            ch.QueueDeclare(queuename, true, false, false, null);

                            //将一个队列和一个路由绑定起来。并制定路由关键字  
                            ch.QueueBind(queuename, RBSendinfo.Exchange, RBSendinfo.RoutingKey);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new RabbitException() { InternalException = ex, QueueName = queuename, RabbitInfo = RBSendinfo.ToString(), CurrentMessage = "" };
                }
                ///构造消息实体对象并发布到消息队列上
                IMapMessageBuilder b = new MapMessageBuilder(ch);
                IDictionary target = b.Headers;
                target["header"] = "RabbitQ";
                IDictionary targerBody = b.Body;
                targerBody["body"] = body;//这个才是具体的发送内容
                if (persistMode)
                {
                    ((IBasicProperties)b.GetContentHeader()).DeliveryMode = 2;
                    //设定传输模式
                }
                //写入
                ch.BasicPublish(RBSendinfo.Exchange, RBSendinfo.RoutingKey, (IBasicProperties)b.GetContentHeader(), b.GetContentBody());
                return true;
            }
        }


        public void Close()
        {
            conn.Close();
        }
        /// <summary>
        /// 判断是否连接有效
        /// </summary>
        public bool IsOpen
        {
            get
            {
                if (conn == null) { return false; }
                return conn.IsOpen;
            }
        }
    }
}
