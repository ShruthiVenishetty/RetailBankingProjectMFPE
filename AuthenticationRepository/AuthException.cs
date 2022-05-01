using System;

namespace AuthenticationRepository
{
    public class AuthException:Exception
    {
        public AuthException(string msg) : base(msg)
        {

        }
    }
}
