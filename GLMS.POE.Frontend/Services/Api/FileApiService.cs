using GLMS.Services.Helpers; // For StreamBrowserFile
using System.Net.Http.Headers;

namespace GLMS.POE.Frontend.Services.Api;

public class FileApiService
{
    private readonly HttpClient _httpClient;

    public FileApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task UploadContractFileAsync(int contractId, StreamBrowserFile file)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(file.OpenReadStream(10 * 1024 * 1024));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        content.Add(fileContent, "file", file.Name);

        var response = await _httpClient.PostAsync($"api/contract/{contractId}/upload", content);
        response.EnsureSuccessStatusCode();
    }
}