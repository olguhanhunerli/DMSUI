using System.Net.Http.Headers;
using DMSUI.Business.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DMSUI.Business;

public class CapaEvidenceFilesApiClient : ICapaEvidenceFilesApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CapaEvidenceFilesApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void AttachToken()
    {
        var token = _httpContextAccessor.HttpContext?
            .Request.Cookies["access_token"];

        _httpClient.DefaultRequestHeaders.Remove("Authorization");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
    public async Task<bool> CreateEvidenceFiles(string capaNo, IFormFile files)
    {
        AttachToken();

        using var content = new MultipartFormDataContent();

        await using var stream = files.OpenReadStream();
        using var fileContent = new StreamContent(stream);

        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(files.ContentType);

        content.Add(fileContent, "File", files.FileName);

        var res = await _httpClient.PostAsync(
            $"api/capa-evidence?capaNo={capaNo}",
            content);

        return res.IsSuccessStatusCode;
    }
}