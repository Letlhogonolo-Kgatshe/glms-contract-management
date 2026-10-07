using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
namespace GLMS.POE.Backend.Services.Interfaces
{
    public interface IClientService
    {
        Task<List<Client>> GetAllClientsAsync();
        Task<Client> GetClientByIdAsync(int id);
        Task<Client> CreateClientAsync(Client client);
        Task<Client> UpdateClientAsync(Client client);
        Task<Client> DeleteClientAsync(int id);


    }

}
