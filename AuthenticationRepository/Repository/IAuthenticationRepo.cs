using AuthenticationRepository.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationRepository.Repository
{
    public interface IAuthenticationRepo
    {
        public Task<AuthResponse> ValidateUser(Credentials credentials);

        public Task<Login> RegisterUser(Register registerDetails);
    }
}
