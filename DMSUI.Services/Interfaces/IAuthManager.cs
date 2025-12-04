using DMSUI.Entities.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IAuthManager
    {
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequest);
        Task<LoginResponseDTO?> RefreshTokenAsync(string refreshToken);
    }
}
