using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailBankingMVC.Controllers
{
    public class EmployeesController : Controller
    {
		public IActionResult Employee()
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else if (HttpContext.Session.GetString("IsEmployee") == "False")
			{
				return RedirectToAction("UnAuthorized", "Authentication");
			}
			else
			{
				return View();
			}
		}
	}
}
