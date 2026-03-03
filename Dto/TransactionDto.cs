using System.ComponentModel.DataAnnotations;

namespace FraudDetection.Dto
{
    public class TransactionRequiredDataDto
    {
        [Required]
        public required string TransactionId { get; set; }
        [Required]
        public required string UserId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime? Timestamp { get; set; }

    }
    public class TransactionDto: TransactionRequiredDataDto
    {
        [Required]
        public required string DeviceId { get; set; }
    }
    public class TransactionDataDto: TransactionRequiredDataDto
    {
        public required string HighRisk { get; set; }

        public required string Suspicious { get; set; }

    }
}
