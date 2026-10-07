using System.Net.Http.Json;

namespace GLMS.POE.Frontend.Services.Api;

public class AuditLogApiService
{
    private readonly HttpClient _httpClient;

    public AuditLogApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<string>> GetAuditLogAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IReadOnlyList<string>>("api/auditlog")
                   ?? new List<string>().AsReadOnly();
        }
        catch (HttpRequestException)
        {
            return new List<string>().AsReadOnly();
        }
    }
}
