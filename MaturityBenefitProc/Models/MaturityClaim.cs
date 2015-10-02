using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturityBenefitProc.Models
{
    public enum ClaimStatus
    {
        Initiated = 0,
        DocumentsPending = 1,
        UnderReview = 2,
        Approved = 3,
        PaymentProcessed = 4,
        Settled = 5,
        Rejected = 6
    }

    public enum PaymentMode
    {
        NEFT = 1,
        Cheque = 2,
        RTGS = 3,
        DemandDraft = 4
    }

    [Table("MaturityClaims")]
    public class MaturityClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Claim number is mandatory")]
        [StringLength(25, MinimumLength = 8)]
        [Index("IX_Claim_Number", IsUnique = true)]
        public string ClaimNumber { get; set; }

        [Required(ErrorMessage = "Policy number is required for maturity claim")]
        [StringLength(20)]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Claimant CIF is required")]
        [StringLength(15)]
        public string ClaimantCif { get; set; }

        [Required]
        public ClaimStatus ClaimStatus { get; set; }

        [Required]
        public PaymentMode PaymentMode { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        [Range(0.01, 999999999.99)]
        public decimal SumAssured { get; set; }

        [Column(TypeName = "decimal")]
        [Range(0, 999999999.99)]
        public decimal AccruedBonus { get; set; }

        [Column(TypeName = "decimal")]
        [Range(0, 999999999.99)]
        public decimal TerminalBonus { get; set; }

        [Column(TypeName = "decimal")]
        [Range(0, 999999999.99)]
        public decimal LoyaltyAddition { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal TotalMaturityAmount { get; set; }

        [Column(TypeName = "decimal")]
        [Range(0, 999999999.99)]
        public decimal TdsAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal NetPayableAmount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime ClaimInitiatedDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime? ApprovalDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime? PaymentDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime? SettlementDate { get; set; }

        [StringLength(50)]
        public string PaymentReference { get; set; }

        [StringLength(500)]
        public string RejectionReason { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("PolicyNumber")]
        public virtual Policy Policy { get; set; }

        [NotMapped]
        public bool IsSettled
        {
            get { return ClaimStatus == ClaimStatus.Settled; }
        }

        [NotMapped]
        public bool IsPendingApproval
        {
            get { return ClaimStatus == ClaimStatus.UnderReview; }
        }

        [NotMapped]
        public decimal TotalBonusComponent
        {
            get { return AccruedBonus + TerminalBonus + LoyaltyAddition; }
        }

        public MaturityClaim()
        {
            ClaimStatus = ClaimStatus.Initiated;
            ClaimInitiatedDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
