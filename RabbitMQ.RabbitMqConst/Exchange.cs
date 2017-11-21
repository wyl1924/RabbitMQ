using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.RabbitMqConst
{
    public class Exchange
    {
        /// <summary>
        /// 路由模式
        /// </summary>
        public const string ExchangeType_Direct = "direct";

        /// <summary>
        /// 广播模式
        /// </summary>
        public const string ExchangeType_Fanout = "fanout";

        /// <summary>
        /// 路由关键字匹配模式
        /// </summary>
        public const string ExchangeType_Topic = "topic";

        /// <summary>
        /// 路由-商城队列
        /// </summary>
        public const string Exchange_ErpAdmin = "erp.service";

    }
}
