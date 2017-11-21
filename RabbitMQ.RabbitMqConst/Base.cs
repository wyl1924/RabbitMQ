using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.RabbitMqConst
{
    public class Base
    {
        /// <summary>
        /// 队列服务器路径
        /// </summary>
        public static string ServerAddress = ConfigurationManager.AppSettings["serveraddress"];

        /// <summary>
        /// vHost
        /// </summary>
        public static string VirtualHost = ConfigurationManager.AppSettings["virtualhost"];
        /// <summary>
        /// 访问用户
        /// </summary>
        public static string UserName = ConfigurationManager.AppSettings["username"];
        /// <summary>
        /// 访问密码
        /// </summary>
        public static string Password = ConfigurationManager.AppSettings["password"];
    }
}
