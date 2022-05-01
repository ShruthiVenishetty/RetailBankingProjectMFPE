using System;

namespace TransactionServiceRepository
{
    public class TransactionServiceException:Exception
    {
        public TransactionServiceException(string msg) : base(msg)
        {

        }
    }
}
