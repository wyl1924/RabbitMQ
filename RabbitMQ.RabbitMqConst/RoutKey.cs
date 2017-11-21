using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.RabbitMqConst
{
    public class RoutKey
    {
        /// <summary>
        /// 路由关键字-库存路由-商城
        /// </summary>
        public const string RoutKey_stock_eshop = "r.stock.eshop";
        /// <summary>
        /// 路由关键字-库存路由-淘宝C  铁血军品行
        /// </summary>
        public const string RoutKey_Stock_TBC = "r.stock.tbc";
        /// <summary>
        /// 路由关键字-库存路由-淘宝b 铁血户外专营店
        /// </summary>
        public const string RoutKey_Stock_TBB = "r.stock.tbb";
        /// <summary>
        /// 路由关键字-库存路由-龙牙B  龙牙旗舰店
        /// </summary>
        public const string RoutKey_Stock_LYB = "r.stock.lyb";
        /// <summary>
        /// 路由关键字-库存路由-京东
        /// </summary>
        public const string RoutKey_Stock_JD = "r.stock.jd";

     
    }
}
