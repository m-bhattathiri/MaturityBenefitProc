using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturityBenefitProc.Models
{
    public enum AccountType
    {
        Savings = 1,
        Current = 2,
        NRE = 3,
        NRO = 4
    }

    [Table("BankDetails")]
    public class BankDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankDetailId { get; set; }

        [Required(ErrorMessage = "CIF number is required")]
        [StringLength(15)]
        public string CifNumber { get; set; }

        [Required(ErrorMessage = "Bank name is required")]
        [StringLength(100)]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(100)]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Account number is required")]
        [StringLength(20, MinimumLength = 8)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "IFSC code is required")]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC code format")]
        public string IfscCode { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [StringLength(9)]
        public string MicrCode { get; set; }

        public bool IsVerified { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime? VerificationDate { get; set; }

        public bool IsPrimary { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("CifNumber")]
        public virtual Policyholder Policyholder { get; set; }

        public BankDetail()
        {
            IsVerified = false;
            IsPrimary = false;
            AccountType = AccountType.Savings;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
