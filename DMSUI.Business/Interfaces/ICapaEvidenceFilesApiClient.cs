using Microsoft.AspNetCore.Http;

namespace DMSUI.Business.Interfaces;

public interface ICapaEvidenceFilesApiClient
{
    Task<bool> CreateEvidenceFiles(string capaNo, IFormFile files);
}