using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturityBenefitProc.Models
{
    public enum KycStatus
    {
        Pending = 0,
        Verified = 1,
        Rejected = 2,
        Expired = 3
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }

    [Table("Policyholders")]
    public class Policyholder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PolicyholderId { get; set; }

        [Required(ErrorMessage = "CIF number is mandatory")]
        [StringLength(15, MinimumLength = 6)]
        [Index("IX_Policyholder_Cif", IsUnique = true)]
        public string CifNumber { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(60)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(60)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "PAN number is mandatory for insurance")]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format")]
        public string PanNumber { get; set; }

        [StringLength(12, MinimumLength = 12)]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhaar must be 12 digits")]
        public string AadhaarNumber { get; set; }

        [Required]
        [StringLength(15)]
        [Phone]
        public string Phone { get; set; }

        [StringLength(120)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        [StringLength(60)]
        public string City { get; set; }

        [Required]
        [StringLength(40)]
        public string State { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be 6 digits")]
        public string Pincode { get; set; }

        [Required]
        public KycStatus KycStatus { get; set; }

        [Column(TypeName = "decimal")]
        [Range(0, 999999999.99)]
        public decimal AnnualIncome { get; set; }

        [StringLength(80)]
        public string Occupation { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }

        public virtual ICollection<BankDetail> BankDetails { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName).Trim(); }
        }

        [NotMapped]
        public int Age
        {
            get
            {
                if (DateOfBirth == default(DateTime))
                    return 0;
                var today = DateTime.UtcNow;
                int age = today.Year - DateOfBirth.Year;
                if (today < DateOfBirth.AddYears(age))
                    age--;
                return age;
            }
        }

        public Policyholder()
        {
            Policies = new HashSet<Policy>();
            BankDetails = new HashSet<BankDetail>();
            KycStatus = KycStatus.Pending;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
