using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Position;
using DMSUI.Entities.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IUserManager
    {
        Task<List<UserListDTO>> GetAllUsersAsync();
        Task<UserListDTO?> GetUserByIdAsync(int userId);
        Task<bool> CreateUserAsync(UserRegisterDTO userRegisterDTO);
        Task<bool> UpdateUserAsync(UserUpdateDTO userUpdateDTO);
		Task<bool> SetActiveStatusAsync(int id, bool activeStatus);
        Task<bool> SoftDeleteUserIdAsync(int id);
        Task<PagedResultDTO<UserListDTO>> SearchUserAsync(UserSearchDTO userSearchDTO);
        Task<bool> UpdatePasswordByUserAsync(PasswordUpdateByUserDTO dto);
        Task<bool> UpdatePasswordByAdminAsync(PasswordResetForAdminDTO dto);
    }
}
