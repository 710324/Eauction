using System;
namespace EAuction.Processor.Exceptions
{
    public class ProductException : Exception
    {
        public ProductException(string message)
            : base(message)
        {
        }

    }
    public class ProductNotFounException : Exception
    {
        public ProductNotFounException(string message)
            : base(message)
        {
        }
    }
}
