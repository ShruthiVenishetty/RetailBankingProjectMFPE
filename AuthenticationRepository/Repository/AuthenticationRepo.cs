using AuthenticationPlugin;
using AuthenticationRepository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationRepository.Repository
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        LoginDBContext dc = new LoginDBContext();
        private readonly SymmetricSecurityKey _key;
        public AuthenticationRepo(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is tadas Secret Key for authentication"));
        }
        public async Task<Login> RegisterUser(Register registerDetails)
        {
            try
            {
                if (registerDetails.Password == null)
                {
                    return null;
                }
                if (registerDetails.Password!=registerDetails.ReEnterPassWord)
                {
                    throw new AuthException("Password and Reenter Password must match");
                    
                }
                Login loginDetails = new Login();
                loginDetails = await (from cred in dc.Logins where cred.Email == registerDetails.Email select cred).SingleOrDefaultAsync();
                if(loginDetails!=null)
                {
                    throw new AuthException("The User with this Email Already Exists");
                }
                else
                {
                    Login newLoginDetails = new Login();
                    Employee employeeLogin = await (from cred in dc.Employees where cred.Email == registerDetails.Email select cred).SingleOrDefaultAsync();
                    Customer customerLogin = await (from cred in dc.Customers where cred.Email == registerDetails.Email select cred).SingleOrDefaultAsync();
                    if (employeeLogin == null && customerLogin == null)
                    {
                        throw new AuthException("No Customer or Employee With That Email Id Present");
                    }
                    else if (employeeLogin != null)
                    {

                        newLoginDetails.UserId = employeeLogin.EmployeeId;
                        newLoginDetails.Email = employeeLogin.Email;
                        newLoginDetails.Role = "Employee";
                        newLoginDetails.Password = SecurePasswordHasherHelper.Hash(registerDetails.Password);
                    }
                    else if (customerLogin != null)
                    {
                        newLoginDetails.UserId = customerLogin.CustomerId;
                        newLoginDetails.Email = customerLogin.Email;
                        newLoginDetails.Role = "Customer";
                        newLoginDetails.Password = SecurePasswordHasherHelper.Hash(registerDetails.Password);
                    }
                    await dc.Logins.AddAsync(newLoginDetails);
                    await dc.SaveChangesAsync();
                    return newLoginDetails;
                }
            }
                
            catch(Exception ex)
            {
                throw new AuthException(ex.Message);
            }

        }

        public async Task<AuthResponse> ValidateUser(Credentials credentials)
        {
            try
            {
                if(credentials.Password==null)
                {
                    return null;
                }
                Login userLogin = await (from cred in dc.Logins where cred.Email == credentials.Email select cred).SingleOrDefaultAsync();

                if (userLogin == null)
                {
                    throw new AuthException("No User With That Email Id Present");
                }
                else
                {
                    if (!SecurePasswordHasherHelper.Verify(credentials.Password, userLogin.Password))
                    {
                        throw new AuthException("Enter Correct Password");
                    }
                    else
                    {
                        List<Claim> claims = new List<Claim>
                        {
                          new Claim(JwtRegisteredClaimNames.Email,userLogin.Email),
                          new Claim(ClaimTypes.Email, userLogin.Email),
                          new Claim(ClaimTypes.Role,userLogin.Role)

                        };
                        var token = GenerateAccessToken(_key, claims);
                        AuthResponse response = new AuthResponse();
                        response.Token = token;
                        response.Role = userLogin.Role;
                        response.UserId= userLogin.UserId;
                        return response;
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw new AuthException(ex.Message);
            }
        }
        public string GenerateAccessToken(SymmetricSecurityKey _key, List<Claim> claims)
        {
            SigningCredentials credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                   claims: claims,
                   issuer: "https://www.Nikhil.com",
                   audience: "https://www.Nikhil.com",
                   expires: DateTime.Now.AddHours(3),
                   signingCredentials: credentials
               );
            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
