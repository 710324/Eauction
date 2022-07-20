using System;

namespace EAuction.Processor.Exceptions
{

    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        {
        }

    }
    public class UserNotFounException : Exception
    {
        public UserNotFounException(string message)
            : base(message)
        {
        }
    }
}

