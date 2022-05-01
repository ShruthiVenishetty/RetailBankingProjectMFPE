using AuthenticationRepository.Models;
using AuthenticationRepository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
       

        private readonly IAuthenticationRepo authRepo;
        public AuthenticateController(IAuthenticationRepo iAuthRepo)
        {

            authRepo = iAuthRepo;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<Login>> Register([FromBody] Register registerDetails)
        {
            try
            {
                
                Login loginDetails = await authRepo.RegisterUser(registerDetails);
                return Ok(loginDetails);
            }
            catch (Exception ex)
            {
               
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] Credentials userCredentials)
        {
            try
            {
                AuthResponse response = await authRepo.ValidateUser( userCredentials);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
