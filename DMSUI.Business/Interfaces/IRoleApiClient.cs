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
    }
}
