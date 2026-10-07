using GLMS.POE.Shared.Models;
using GLMS.POE.Backend.Services.Interfaces;
using GLMS.POE.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace GLMS.POE.Backend.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _db;

        public ClientService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _db.Clients
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _db.Clients.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException("Client not found.");
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            return client;
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();

            return client;
        }

        public async Task<Client> DeleteClientAsync(int id)
        {
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException("Client not found.");

            _db.Clients.Remove(client);
            await _db.SaveChangesAsync();

            return client;
        }
    }
}
