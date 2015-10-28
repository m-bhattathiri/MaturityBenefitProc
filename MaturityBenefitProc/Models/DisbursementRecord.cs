using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturityBenefitProc.Models
{
    public enum DisbursementStatus
    {
        Initiated = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3,
        Reversed = 4,
        OnHold = 5
    }

    public enum DisbursementChannel
    {
        NEFT = 1,
        RTGS = 2,
        Cheque = 3,
        DemandDraft = 4
    }

    [Table("DisbursementRecords")]
    public class DisbursementRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DisbursementId { get; set; }

        [Required(ErrorMessage = "Claim number is required")]
        [StringLength(25)]
        public string ClaimNumber { get; set; }

        [Required(ErrorMessage = "Policy number is required")]
        [StringLength(20)]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Beneficiary CIF is required")]
        [StringLength(15)]
        public string BeneficiaryCif { get; set; }

        [Required]
        public DisbursementChannel Channel { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        [Range(0.01, 999999999.99, ErrorMessage = "Disbursement amount is out of range")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal")]
        [Range(0, 999999999.99)]
        public decimal TdsDeducted { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal NetAmount { get; set; }

        [StringLength(20)]
        public string BankAccountNumber { get; set; }

        [StringLength(11)]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC code format")]
        public string IfscCode { get; set; }

        [StringLength(100)]
        public string BankName { get; set; }

        [StringLength(10)]
        public string ChequeNumber { get; set; }

        [StringLength(30)]
        public string CourierAwb { get; set; }

        [StringLength(30)]
        public string UtrNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime DisbursementDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime? CompletionDate { get; set; }

        [Required]
        public DisbursementStatus Status { get; set; }

        [StringLength(500)]
        public string FailureReason { get; set; }

        [Range(0, 5)]
        public int RetryCount { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("ClaimNumber")]
        public virtual MaturityClaim MaturityClaim { get; set; }

        [NotMapped]
        public bool IsCompleted
        {
            get { return Status == DisbursementStatus.Completed; }
        }

        [NotMapped]
        public bool CanRetry
        {
            get { return Status == DisbursementStatus.Failed && RetryCount < 3; }
        }

        public DisbursementRecord()
        {
            Status = DisbursementStatus.Initiated;
            RetryCount = 0;
            DisbursementDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
