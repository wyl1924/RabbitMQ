using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class RabbitException : Exception
    {
        public Exception InternalException { get; set; }

        /// <summary>
        /// Rabbit Server Address Port Vhost Exchange ExchangeType 
        /// </summary>
        public string RabbitInfo { get; set; }

        public string QueueName { get; set; }

        public string CurrentMessage { get; set; }

        public override string ToString()
        {
            return "Exception" + InternalException.ToString() + "RabbitInfo:" + RabbitInfo + ",QueueName:" + QueueName + ",CurrentMessage:" + CurrentMessage;
        }
    }



    public class NotFanoutExchangeTypeException : Exception
    {
        public override string Message
        {
            get
            {
                return "ExchengeType must be fanout!";
            }
        }
        public override string ToString()
        {
            return string.Format("ExchengeType must be fanout!{0}", base.ToString());
        }
    }

    public class UnExpectedMessageExceptiion : Exception
    {
        public override string Message
        {
            get
            {
                return "UnExpected Message!";
            }
        }

        public override string ToString()
        {
            return string.Format("UnExpected Message!{0}", base.ToString());
        }

    }
}
