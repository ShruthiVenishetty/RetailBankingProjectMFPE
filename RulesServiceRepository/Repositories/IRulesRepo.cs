using RulesServiceRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesServiceRepository.Repositories
{
    public interface IRulesRepo
    {
        Task<RuleStatus> EvaluateMinBalance(int acountId, decimal balance, decimal ammountRequest);

        float GetServiceCharges(string TransactionMode);
    }
}
