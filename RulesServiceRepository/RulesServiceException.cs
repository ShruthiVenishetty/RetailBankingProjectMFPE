using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesServiceRepository
{
    public class RulesServiceException: Exception
    {
        public RulesServiceException(string msg) : base(msg)
        {

        }
    }
}
