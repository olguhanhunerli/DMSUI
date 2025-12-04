using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Role;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
	public class RoleManager : IRoleManager
	{
		private readonly IRoleApiClient _roleApiClient;

		public RoleManager(IRoleApiClient roleApiClient)
		{
			_roleApiClient = roleApiClient;
		}

		public async Task<List<RoleListDTO>> GetAllRolesAsync()
		{
			return await _roleApiClient.GetRoleListsAsync();
		}
	}
}
