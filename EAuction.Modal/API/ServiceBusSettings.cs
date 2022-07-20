using System;
using System.Collections.Generic;
using System.Text;

namespace EAuction.Models.API
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        
        public string QueueName { get; set; }

        public int NumOfMessages { get; set; }
    }
}
