using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class RabbitConsumer
    {

        private RabbitConsumerConfig RBGetinfo;
        private ConnectionFactory cf = new ConnectionFactory();
        private IConnection conn; //建立联接

        /// <summary>
        /// 初始化Rabbit连接
        /// </summary>
        /// <param name="rbinfo"></param>
        public RabbitConsumer(RabbitConsumerConfig rbinfo)
        {
            RBGetinfo = rbinfo;
            cf = new ConnectionFactory()
            {
                UserName = RBGetinfo.UserName,
                Password = RBGetinfo.Password,
                VirtualHost = RBGetinfo.VirtualHost,
                RequestedHeartbeat = 0,
                Uri = RBGetinfo.ServerAddress
            };
            conn = cf.CreateConnection();

        }

        private void CheckConn()
        {
            if (RBGetinfo != null && !IsOpen)
            {
                cf = new ConnectionFactory()
                {
                    UserName = RBGetinfo.UserName,
                    Password = RBGetinfo.Password,
                    VirtualHost = RBGetinfo.VirtualHost,
                    RequestedHeartbeat = 0,
                    Uri = RBGetinfo.ServerAddress
                };
                conn = cf.CreateConnection();
            }
        }

        /// <summary>
        /// 初始化Rabbit连接，此方法只在测试时使用
        /// </summary>
        /// <param name="rbinfo"></param>
        public RabbitConsumer(string username, string password, string virtualhost, string serveraddress)
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
        }


        /// <summary>
        /// 队列出列的方法，传入处理队列中body的方法，并传入队列名称
        /// </summary>
        /// <param name="messageProcessAction"></param>
        /// <param name="queuename"></param>
        public void ConsumeMessage(Action<string> messageProcessAction, string queuename)
        {
            if (string.IsNullOrEmpty(queuename))
            {
                throw new ArgumentNullException("queuename");
            }
            CheckConn();
            using (IModel ch = conn.CreateModel())
            {
                //第二种取法QueueingBasicConsumer基于订阅模式
                QueueingBasicConsumer consumer = new QueueingBasicConsumer(ch);
                ch.BasicQos(0, 1, true);
                ch.BasicConsume(queuename, false, consumer);
                while (true)
                {
                    string message = "";
                    try
                    {
                        BasicDeliverEventArgs e = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        IBasicProperties props = e.BasicProperties;
                        byte[] body = e.Body;
                        message = System.Text.Encoding.UTF8.GetString(body);
                        //MessageBox.Show(System.Text.Encoding.UTF8.GetString(body));
                        //logger.Info(System.Text.Encoding.UTF8.GetString(body).Replace("\0\0\0body\0\n", "").ToString());
                        messageProcessAction.Invoke(System.Text.Encoding.UTF8.GetString(body).Replace("\0\0\0body\0\n", "").Replace("\0", "").ToString());
                        ch.BasicAck(e.DeliveryTag, false);
                        //ProcessRemainMessage();
                    }
                    catch (Exception ex)
                    {
                        throw new RabbitException() { InternalException = ex, QueueName = queuename, RabbitInfo = RBGetinfo.ToString(), CurrentMessage = message };
                    }
                }
            }
        }
        /// <summary>
        /// 队列出列的方法，传入处理队列中body的方法，并传入队列名称
        /// </summary>
        /// <param name="messageProcessAction"></param>
        /// <param name="queuename"></param>
        public void ConsumeMessage(Action<string> messageProcessAction, string queuename, ushort count)
        {
            if (string.IsNullOrEmpty(queuename))
            {
                throw new ArgumentNullException("queuename");
            }
            CheckConn();
            using (IModel ch = conn.CreateModel())
            {
                //第二种取法QueueingBasicConsumer基于订阅模式
                QueueingBasicConsumer consumer = new QueueingBasicConsumer(ch);
                ch.BasicQos(0, count, true);
                ch.BasicConsume(queuename, false, consumer);
                while (true)
                {
                    string message = "";
                    try
                    {
                        BasicDeliverEventArgs e = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        IBasicProperties props = e.BasicProperties;
                        byte[] body = e.Body;
                        message = System.Text.Encoding.UTF8.GetString(body);
                        messageProcessAction.Invoke(System.Text.Encoding.UTF8.GetString(body).Replace("\0\0\0body\0\n", "").Replace("\0", "").ToString());
                        ch.BasicAck(e.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        throw new RabbitException() { InternalException = ex, QueueName = queuename, RabbitInfo = RBGetinfo.ToString(), CurrentMessage = message };
                    }
                }
            }
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
