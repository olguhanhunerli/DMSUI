using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface IRoleApiClient
    {
        Task<List<RoleListDTO>> GetRoleListsAsync();
        Task<RoleListDTO> GetRoleByIdAsync(int id);
		Task<bool> CreateRoleAsync(RoleCreateDTO roleCreateDTO);

		Task<bool> UpdateRoleAsync(int id, RoleUpdateDTO roleUpdateDTO);
        Task<bool> DeleteRoleAsync(int id);
        Task<PagedResultDTO<RoleListDTO>> GetPagedAsync(int page, int pageSize);
    }
}
