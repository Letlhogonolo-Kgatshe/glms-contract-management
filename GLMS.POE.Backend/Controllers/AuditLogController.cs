using GLMS.POE.Backend.Services.Implementations;
using GLMS.POE.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.POE.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<string>> Get()
        {
            return Ok(ContractObserver.AuditLog);
        }

        //[HttpGet("recent")]
        //public async Task<ActionResult<IEnumerable<AuditLog>>> GetRecent(int count = 50)
        //{
        //    var logs = await _auditLogService.GetRecentAuditLogsAsync(count);
        //    return Ok(logs);
        //}
    }
}
