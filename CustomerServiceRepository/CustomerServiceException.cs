using System;

namespace CustomerServiceRepository
{
    public class CustomerServiceException : Exception
    {
        public CustomerServiceException(string msg) : base(msg)
        {

        }
    }
}
