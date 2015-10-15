using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturityBenefitProc.Models
{
    public enum NomineeRelation
    {
        Spouse = 1,
        Child = 2,
        Parent = 3,
        Sibling = 4,
        Other = 5
    }

    [Table("Nominees")]
    public class Nominee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NomineeId { get; set; }

        [Required(ErrorMessage = "Policy number is required")]
        [StringLength(20)]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Nominee name is required")]
        [StringLength(120)]
        public string NomineeName { get; set; }

        [Required]
        public NomineeRelation NomineeRelation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(0.01, 100.00, ErrorMessage = "Share percentage must be between 0.01 and 100")]
        [Column(TypeName = "decimal")]
        public decimal SharePercentage { get; set; }

        [StringLength(120)]
        public string GuardianName { get; set; }

        [StringLength(60)]
        public string GuardianRelation { get; set; }

        [StringLength(15)]
        [Phone]
        public string ContactNumber { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public bool KycVerified { get; set; }

        [StringLength(40)]
        public string IdentityProof { get; set; }

        [StringLength(30)]
        public string IdentityProofNumber { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("PolicyNumber")]
        public virtual Policy Policy { get; set; }

        [NotMapped]
        public bool IsMinor
        {
            get
            {
                if (DateOfBirth == default(DateTime))
                    return false;
                var today = DateTime.UtcNow;
                int age = today.Year - DateOfBirth.Year;
                if (today < DateOfBirth.AddYears(age))
                    age--;
                return age < 18;
            }
        }

        [NotMapped]
        public bool RequiresGuardian
        {
            get { return IsMinor && string.IsNullOrWhiteSpace(GuardianName); }
        }

        public Nominee()
        {
            KycVerified = false;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
