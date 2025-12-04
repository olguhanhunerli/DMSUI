using DMSUI.Entities.DTOs.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface IDepartmentApiClient
    {
		Task<List<DepartmentListDTO>> GetAllDepartmentsAsync();
	}
}
