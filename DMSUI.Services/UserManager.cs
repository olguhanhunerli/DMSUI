using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Position;
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

        public async Task<bool> CreateUserAsync(UserRegisterDTO userRegisterDTO)
        {
            return await _userApiClient.CreateUserAsync(userRegisterDTO);
        }

        public async Task<List<UserListDTO>> GetAllUsersAsync()
        {
            return await _userApiClient.GetAllUsersAsync();
        }

        public async Task<UserListDTO?> GetUserByIdAsync(int userId)
        {
            return await _userApiClient.GetUserByIdAsync(userId);
        }

        public async Task<PagedResultDTO<UserListDTO>> SearchUserAsync(UserSearchDTO userSearchDTO)
        {
            return await _userApiClient.SearchUserAsync(userSearchDTO);
        }

        public async Task<bool> SetActiveStatusAsync(int id, bool activeStatus)
		{
			return await _userApiClient.SetActiveStatusAsync(id, activeStatus);
		}

        public async Task<bool> SoftDeleteUserIdAsync(int id)
        {
            return await _userApiClient.SoftDeleteUserIdAsync(id);
        }

        public async Task<bool> UpdatePasswordByAdminAsync(PasswordResetForAdminDTO dto)
        {
            return await _userApiClient.UpdatePasswordByAdminAsync(dto);
        }

        public async Task<bool> UpdatePasswordByUserAsync(PasswordUpdateByUserDTO dto)
        {
            return await _userApiClient.UpdatePasswordByUserAsync(dto);
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDTO userUpdateDTO)
		{
			return await _userApiClient.UpdateUserAsync(userUpdateDTO);
		}
	}
}
