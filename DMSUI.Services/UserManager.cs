using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.User;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserApiClient _userApiClient;

        public UserManager(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task<List<UserListDTO>> GetAllUsersAsync()
        {
            return await _userApiClient.GetAllUsersAsync();
        }

        public async Task<UserListDTO?> GetUserByIdAsync(int userId)
        {
            return await _userApiClient.GetUserByIdAsync(userId);
        }
    }
}
