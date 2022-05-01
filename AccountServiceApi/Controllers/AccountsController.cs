using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServiceRepository.Models;
using AccountServiceRepository.Repository;
using AccountServiceApi.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace AccountServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepo accountRepo;
        public AccountsController(IAccountRepo iaccountRepo)
        { 
            accountRepo = iaccountRepo;
        }
        [HttpGet("GetCustomerAccounts/{customerId}")]
        [Authorize(Roles = "Employee,Customer")]
        public async Task<ActionResult<List<Account>>> GetCustomerAccounts(int customerId)
        {
            try
            {
                List<Account> customerAccounts = await accountRepo.GetAccountDetailsBycid(customerId);
                if (customerAccounts.Count != 0)
                {
                    return Ok(customerAccounts);
                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    
        [HttpGet("{accountId}")]
        public async Task<ActionResult<Account>> GetAccount(int accountId)
        {
            try
            {
                Account accountDetails = await accountRepo.GetAccountDetailsById(accountId);
                if (accountDetails != null)
                {
                    return Ok(accountDetails);
                }
                else
                {
                    return NotFound("No account with that Id Found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<AccountCreationStatus>> createAccount([FromBody] CustomerRequest customerRequest)
        {
            try
            {
                AccountCreationStatus accountCreationStatus = await accountRepo.InsertAccount(customerRequest.CustomerId);

                return Ok(accountCreationStatus);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
    }
}
