using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
	public class DepartmentManager : IDepartmentManager
	{
		private readonly IDepartmentApiClient _departmentApiClient;

		public DepartmentManager(IDepartmentApiClient departmentApiClient)
		{
			_departmentApiClient = departmentApiClient;
		}

		public async Task<bool> CreateDepartmentAsync(DepartmentCreateDTO department)
		{
			return await _departmentApiClient.CreateDepartmentAsync(department);
		}

		public async Task<bool> DeleteDepartmentAsync(int id)
		{
			return await _departmentApiClient.DeleteDepartmentAsync(id);
		}

		public async Task<List<DepartmentListDTO>> GetAllDepartmentsAsync()
		{
			return await _departmentApiClient.GetAllDepartmentsAsync();
		}

		public async Task<DepartmentDetailDTO> GetDepartmentsByIdAsync(int id)
		{
			return await _departmentApiClient.GetDepartmentsByIdAsync(id);
		}

        public async Task<PagedResultDTO<DepartmentListDTO>> GetPagedAsync(int page, int pageSize)
        {
            return await _departmentApiClient.GetPagedAsync(page, pageSize);
        }

        public async Task<bool> UpdateDepartmentAsync(DepartmentUpdateDTO department)
		{
			return await _departmentApiClient.UpdateDepartmentAsync(department);
		}
	}
}
