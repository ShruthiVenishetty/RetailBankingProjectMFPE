using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailBankingMVC.Models.AuthenticationModels;
using RetailBankingMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RetailBankingMVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService authService;

        public AuthenticationController(IAuthenticationService _authService)
        {

            authService = _authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginData loginData)
        {
                AuthResponse authResponse = new AuthResponse();
                HttpResponseMessage response = await authService.Login(loginData);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonContent = await response.Content.ReadAsStringAsync();
                    authResponse = JsonConvert.DeserializeObject<AuthResponse>(JsonContent);
                    HttpContext.Session.SetString("token", authResponse.Token);
                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(loginData));
                    HttpContext.Session.SetInt32("userId", authResponse.UserId);
                    HttpContext.Session.SetString("email", loginData.Email);
                    if (authResponse.Role == "Employee")
                    {
                        HttpContext.Session.SetString("IsEmployee", "True");
                        return RedirectToAction("Employee", "Employees");
                    }
                    else
                    {
                        HttpContext.Session.SetString("IsCustomer", "True");
                        return RedirectToAction("Index", "Customers");
                    }


                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Errormessage = response.Content.ReadAsStringAsync().Result;
                    return View();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    ViewBag.Errormessage = "Internal Server Error! Please try again later";
                    return View();
                }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterData registerData)
        {
           
                LoginData loginData = new LoginData();
                HttpResponseMessage response = await authService.Register(registerData);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonContent = await response.Content.ReadAsStringAsync();
                    loginData = JsonConvert.DeserializeObject<LoginData>(JsonContent);
                    return View("Login", loginData);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Errormessage = response.Content.ReadAsStringAsync().Result;
                    return View();
                }     
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    ViewBag.Errormessage = "Internal Server Error! Please try again later";
                    return View();
                }

                return View();
          
        }
        public ActionResult Logout()
        {

            HttpContext.Session.Clear();

            return View("Login");
        }


        [HttpGet]
        public IActionResult UnAuthorized()
        {
            return View();
        }
    }
}
