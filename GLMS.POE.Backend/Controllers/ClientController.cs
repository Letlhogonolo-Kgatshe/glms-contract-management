using GLMS.POE.Backend.Services.Interfaces;
using GLMS.POE.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.POE.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        {
            Console.WriteLine(">>> GET /api/clients called");
            var clients = await _clientService.GetAllClientsAsync();
            Console.WriteLine($">>> Returning {clients.Count} clients");
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetById(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            return client == null ? NotFound() : Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Create(Client client)
        {
            await _clientService.CreateClientAsync(client);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Client client)
        {
            if (id != client.Id) return BadRequest("ID mismatch");
            await _clientService.UpdateClientAsync(client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return NoContent();
        }
    }
}
