using DMSUI.Business.Interfaces;
using DMSUI.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DMSUI.Services;

public class CapaEvidenceFilesManager: ICapaEvidenceFilesManager
{
    private readonly ICapaEvidenceFilesApiClient _capaEvidenceFilesApiClient;

    public CapaEvidenceFilesManager(ICapaEvidenceFilesApiClient capaEvidenceFilesApiClient)
    {
        _capaEvidenceFilesApiClient = capaEvidenceFilesApiClient;
    }

    public async Task<bool> CreateEvidenceFiles(string capaNo, IFormFile files)
    {
        return await _capaEvidenceFilesApiClient.CreateEvidenceFiles(capaNo, files);
    }
}