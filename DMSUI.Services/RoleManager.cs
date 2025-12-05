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

		public async Task<bool> CreateRoleAsync(RoleCreateDTO roleCreateDTO)
		{
			return await _roleApiClient.CreateRoleAsync(roleCreateDTO);
		}

		public async Task<bool> DeleteRoleAsync(int id)
		{
			return await _roleApiClient.DeleteRoleAsync(id);
		}

		public async Task<List<RoleListDTO>> GetAllRolesAsync()
		{
			return await _roleApiClient.GetRoleListsAsync();
		}

		public async Task<RoleListDTO> GetRoleByIdAsync(int id)
		{
			return await _roleApiClient.GetRoleByIdAsync(id);
		}

		public async Task<bool> UpdateRoleAsync(int id, RoleUpdateDTO roleUpdateDTO)
		{
			return await _roleApiClient.UpdateRoleAsync(id, roleUpdateDTO);
		}
	}
}
