using System;
using System.Collections.Generic;
using System.Text;

namespace EAuction.Models.API
{
    public class MessagePayload
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; }
    }
}
