using GLMS.POE.Shared.Models;
using GLMS.POE.Backend.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace GLMS.POE.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestService _serviceRequestService;

        public ServiceRequestController(IServiceRequestService serviceRequestService)
        {
            _serviceRequestService = serviceRequestService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetAll()
        {
            var requests = await _serviceRequestService.GetAllServiceRequestsAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetById(int id)
        {
            var request = await _serviceRequestService.GetServiceRequestByIdAsync(id);
            return request == null ? NotFound() : Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> Create(ServiceRequest request)
        {
            await _serviceRequestService.CreateServiceRequestAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            await _serviceRequestService.UpdateServiceRequestAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceRequestService.DeleteServiceRequestAsync(id);
            return NoContent();
        }

        [HttpGet("by-contract/{contractId}")]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetByContract(int contractId)
        {
            var requests = await _serviceRequestService.GetServiceRequestsByContractAsync(contractId);
            return Ok(requests);
        }
    }
}
