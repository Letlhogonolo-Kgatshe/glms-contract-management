using GLMS.POE.Shared.Models;
using GLMS.POE.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.POE.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetAll()
        {
            var contracts = await _contractService.GetAllContractsAsync();
            return Ok(contracts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetById(int id)
        {
            var contract = await _contractService.GetContractByIdAsync(id);
            return contract == null ? NotFound() : Ok(contract);
        }

        [HttpPost]
        public async Task<ActionResult<Contract>> Create(Contract contract)
        {
            await _contractService.CreateContractAsync(contract);
            return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Contract contract)
        {
            if (id != contract.Id) return BadRequest("ID mismatch");
            await _contractService.UpdateContractAsync(contract);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contractService.DeleteContractAsync(id);
            return NoContent();
        }

        [HttpGet("by-client/{clientId}")]
        public async Task<ActionResult<IEnumerable<Contract>>> GetByClient(int clientId)
        {
            var contracts = await _contractService.GetContractsByClientAsync(clientId);
            return Ok(contracts);
        }
    }
}
