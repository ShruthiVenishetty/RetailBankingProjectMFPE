using RulesServiceRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesServiceRepository.Repositories
{
    public class RulesRepo : IRulesRepo
    {
        public async Task<RuleStatus> EvaluateMinBalance(int acountId, decimal balance, decimal ammountRequest)
        {
           try
           {
                RuleStatus rulesStatus = new RuleStatus();
                if(balance<500)
                {
                    rulesStatus.Status = "Declined";
                }
                else if(ammountRequest>balance-500-((85* ammountRequest)/10000))
                {
                    rulesStatus.Status = "Declined";
                }
                else
                {
                    rulesStatus.Status = "Allowed";
                }
                return rulesStatus;
           }
           catch(Exception)
           {
                throw new RulesServiceException("Error While Performing Minimum Balance Check");
           }
        }

        public float GetServiceCharges(string transactionMode)
        {
            try
            {

                if (transactionMode == "Withdraw")
                {
                    return 0.85f;
                }
                else if (transactionMode == "Deposit")
                {
                    return 0.05f;
                }
                return 0.00f;
            }
            catch (Exception)
            {
                throw new RulesServiceException("Error While getting Service Charge Details");
            }
        }
    }
}
