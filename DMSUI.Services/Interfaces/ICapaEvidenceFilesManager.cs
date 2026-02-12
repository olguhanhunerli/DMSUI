using Microsoft.AspNetCore.Http;

namespace DMSUI.Services.Interfaces;

public interface ICapaEvidenceFilesManager
{
    Task<bool> CreateEvidenceFiles(string capaNo, IFormFile files);
}