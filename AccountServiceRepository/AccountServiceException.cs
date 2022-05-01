using System;

namespace AccountServiceRepository
{
    public class AccountServiceException:Exception
    {
        public AccountServiceException(string msg) : base(msg)
        {

        }
    }
}
