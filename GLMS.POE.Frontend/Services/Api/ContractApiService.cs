using GLMS.POE.Shared.Models;
using System.Net.Http.Json;

namespace GLMS.POE.Frontend.Services.Api;

public class ContractApiService
{
    private readonly HttpClient _httpClient;

    public ContractApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
    }

    public async Task<List<Contract>> GetAllContractsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<Contract>>("api/contract")
                   ?? new List<Contract>();
        }
        catch (HttpRequestException)
        {
            return new List<Contract>();
        }
    }

    public async Task<Contract?> GetContractByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Contract>($"api/contract/{id}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<Contract?> CreateContractAsync(Contract contract, bool createAsActive = false)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"api/contract?createAsActive={createAsActive}", contract);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Contract>();
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task UpdateContractAsync(Contract contract)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/contract/{contract.Id}", contract);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException) { }
    }

    public async Task DeleteContractAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/contract/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException) { }
    }
}