using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Auth;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IAuthApiClient _authApiClient;

        public AuthManager(IAuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequest)
        {
            return await _authApiClient.LoginAsync(loginRequest);
        }

        public async Task<LoginResponseDTO?> RefreshTokenAsync(string refreshToken)
        {
           return await _authApiClient.RefreshTokenAsync(refreshToken);
        }
    }
}
