using DMSUI.Business.Interfaces;
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

		public Task<List<DepartmentListDTO>> GetAllDepartmentsAsync()
		{
			return _departmentApiClient.GetAllDepartmentsAsync();
		}
	}
}
