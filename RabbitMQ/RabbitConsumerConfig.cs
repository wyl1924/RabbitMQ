using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.RabbitMQ
{
    /// <summary>
    /// 消费者的实体类
    /// </summary>
    public class RabbitConsumerConfig
    {
        public string UserName { get; set; } //连接帐号
        public string Password { get; set; } //连接密码
        public string VirtualHost { get; set; }      //虚拟主机名   
        public string ServerAddress { get; set; }      //服务器地址 

        public RabbitConsumerConfig()
        {
            ServerAddress = ConfigurationManager.AppSettings["serveraddress"];
            VirtualHost = ConfigurationManager.AppSettings["virtualhost"];
            UserName = ConfigurationManager.AppSettings["username"];
            Password = ConfigurationManager.AppSettings["password"];
        }
        public override string ToString()
        {
            return "UserName:" + UserName + ",Password:" + Password + ",VirtualHost:" + VirtualHost + ",ServerAddress:" + ServerAddress;
        }
    }
}
