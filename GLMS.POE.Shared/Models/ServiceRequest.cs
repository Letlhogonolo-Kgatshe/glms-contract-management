using System.ComponentModel.DataAnnotations;
using GLMS.POE.Shared.Models.Enums;

namespace GLMS.POE.Shared.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        [Required]
        public int ContractId { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than zero")]
        public decimal CostUsd { get; set; }
        public decimal CostZar { get; set; }
        public decimal ExchangeRateUsed { get; set; }
        [Required]
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Contract? Contract { get; set; }
    }
}
