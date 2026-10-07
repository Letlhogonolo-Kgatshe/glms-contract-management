using GLMS.POE.Shared.Models;
using GLMS.POE.Frontend.Services.Interfaces;
using System.Net.Http.Json;

namespace GLMS.POE.Frontend.Services.Api;

public class ServiceRequestApiService : IServiceRequestService
{
    private readonly HttpClient _httpClient;

    public ServiceRequestApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ServiceRequest>> GetAllAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequest")
                   ?? new List<ServiceRequest>();
        }
        catch (HttpRequestException)
        {
            return new List<ServiceRequest>();
        }
    }

    public async Task<List<ServiceRequest>> GetAllServiceRequestsAsync() => await GetAllAsync();

    public async Task<ServiceRequest?> GetByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ServiceRequest>($"api/servicerequest/{id}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<(bool success, string error)> CreateAsync(ServiceRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/servicerequest", request);
            if (response.IsSuccessStatusCode) return (true, "");
            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (HttpRequestException ex)
        {
            return (false, $"Connection failed: {ex.Message}");
        }
    }
}
