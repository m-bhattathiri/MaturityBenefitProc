using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturityBenefitProc.Models
{
    public enum PolicyType
    {
        Endowment = 1,
        MoneyBack = 2,
        WholeLife = 3,
        TermPlan = 4,
        ULIP = 5
    }

    public enum PolicyStatus
    {
        Active = 1,
        Matured = 2,
        Lapsed = 3,
        Surrendered = 4,
        PaidUp = 5,
        Cancelled = 6
    }

    public enum PremiumFrequency
    {
        Monthly = 1,
        Quarterly = 2,
        HalfYearly = 3,
        Yearly = 4
    }

    [Table("Policies")]
    public class Policy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Policy number is mandatory")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Policy number must be between 8 and 20 characters")]
        [Index("IX_Policy_Number", IsUnique = true)]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Policyholder CIF is required")]
        [StringLength(15)]
        public string PolicyholderCif { get; set; }

        [Required]
        public PolicyType PolicyType { get; set; }

        [Required]
        public PolicyStatus PolicyStatus { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        [Range(10000, 100000000, ErrorMessage = "Sum assured must be between 10,000 and 10,00,00,000")]
        public decimal SumAssured { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        [Range(500, 50000000, ErrorMessage = "Premium amount is out of acceptable range")]
        public decimal PremiumAmount { get; set; }

        [Required]
        public PremiumFrequency PremiumFrequency { get; set; }

        [Required]
        [Range(5, 75, ErrorMessage = "Policy term must be between 5 and 75 years")]
        public int PolicyTerm { get; set; }

        [Required]
        [Range(5, 75, ErrorMessage = "Premium paying term must be between 5 and 75 years")]
        public int PremiumPayingTerm { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime CommencementDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime MaturityDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime? LastPremiumDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(30)]
        public string RiskCategory { get; set; }

        [Required]
        [StringLength(10)]
        public string BranchCode { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Nominee> Nominees { get; set; }

        [ForeignKey("PolicyholderCif")]
        public virtual Policyholder Policyholder { get; set; }

        [NotMapped]
        public bool IsMatured
        {
            get { return PolicyStatus == PolicyStatus.Matured; }
        }

        [NotMapped]
        public int YearsCompleted
        {
            get
            {
                if (CommencementDate == default(DateTime))
                    return 0;
                var today = DateTime.UtcNow;
                int years = today.Year - CommencementDate.Year;
                if (today < CommencementDate.AddYears(years))
                    years--;
                return Math.Max(0, years);
            }
        }

        [NotMapped]
        public int RemainingYears
        {
            get
            {
                int remaining = PolicyTerm - YearsCompleted;
                return Math.Max(0, remaining);
            }
        }

        public Policy()
        {
            Nominees = new HashSet<Nominee>();
            PolicyStatus = PolicyStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
