using DMSUI.Business.Interfaces;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class CapaActionFilesManager : ICapaActionFilesManager
    {
        private readonly ICapaActionFilesApiClient _apiClient;

        public CapaActionFilesManager(ICapaActionFilesApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> CreateFile(int actionId, IFormFile file)
        {
           return await _apiClient.CreateFile(actionId, file);
        }
    }
}
