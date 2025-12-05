using DMSUI.Entities.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IRoleManager
    {
        Task<List<RoleListDTO>> GetAllRolesAsync();
		Task<RoleListDTO> GetRoleByIdAsync(int id);
		Task<bool> CreateRoleAsync(RoleCreateDTO roleCreateDTO);
		Task<bool> UpdateRoleAsync(int id, RoleUpdateDTO roleUpdateDTO);
		Task<bool> DeleteRoleAsync(int id);

	}
}
