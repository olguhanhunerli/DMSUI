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
    }
}
