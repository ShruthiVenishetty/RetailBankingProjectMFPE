using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RulesServiceRepository.Models;
using RulesServiceRepository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IRulesRepo rulesRepo;
        public RulesController(IRulesRepo iRulesRepo)
        {

            rulesRepo = iRulesRepo;
        }
        [HttpGet("EvaluateMinBal/AccountId-{acountId}/balance-{balance}/ammountRequest-{ammountRequest}")]
        public async Task<ActionResult<RuleStatus>> EvaluateMinBal(int acountId,decimal balance,decimal ammountRequest)
        {
            try
            {

                RuleStatus ruleStatus = await rulesRepo.EvaluateMinBalance(acountId,balance,ammountRequest);
                return Ok(ruleStatus);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
      
    }
}
