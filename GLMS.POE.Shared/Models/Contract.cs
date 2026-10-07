using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GLMS.POE.Shared.Models.Enums;

namespace GLMS.POE.Shared.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required]
        public ContractStatus Status { get; set; } = ContractStatus.Draft;

        [Required(ErrorMessage = "Service level is required")]
        [StringLength(100)]
        public string ServiceLevel { get; set; } = string.Empty;

        // File handling: path to uploaded signed agreement PDF
        [StringLength(500)]
        public string? SignedAgreementPath { get; set; }

        [StringLength(255)]
        public string? SignedAgreementFileName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Client? Client { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
