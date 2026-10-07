using GLMS.POE.Shared.Models;
using System.Net.Http.Json;

namespace GLMS.POE.Frontend.Services.Api;

public class ClientApiService
{
    private readonly HttpClient _httpClient;

    public ClientApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Client>> GetAllClientsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<Client>>("api/client")
                   ?? new List<Client>();
        }
        catch (HttpRequestException)
        {
            return new List<Client>();
        }
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Client>($"api/client/{id}");
    }
    public async Task CreateClientAsync(Client client)
    {
        var response = await _httpClient.PostAsJsonAsync("api/client", client);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateClientAsync(Client client)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/client/{client.Id}", client);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteClientAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/client/{id}");
        response.EnsureSuccessStatusCode();
    }
}