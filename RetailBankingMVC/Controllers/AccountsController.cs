using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailBankingMVC.Models.AccountModels;
using RetailBankingMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailBankingMVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        string token;
		public AccountsController(IAccountService accountService)
        {
			
			_accountService = accountService;
        }
		[HttpGet]
		public async Task<IActionResult> GetCustomerAccount(int customerId)
		{
			token = HttpContext.Session.GetString("token");
			if (HttpContext.Session.GetString("IsEmployee") != "True" && HttpContext.Session.GetString("IsCustomer") != "True")
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				List<Account> accounts = new List<Account>();
					HttpResponseMessage response = await _accountService.GetAccountDetailsByCid(customerId,token);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var JsonContent = await response.Content.ReadAsStringAsync();
						accounts = JsonConvert.DeserializeObject<List<Account>>(JsonContent);
						ViewBag.menu = accounts;
						return View(accounts);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
					{
						ViewBag.Message = "Invalid Customer ID";
						return View(accounts);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					{
						ViewBag.Message = "Internal Server Error! Please try again later";
						return View(accounts);
					}
				
				return View(accounts);
			}
		}
		
		
	}
}
