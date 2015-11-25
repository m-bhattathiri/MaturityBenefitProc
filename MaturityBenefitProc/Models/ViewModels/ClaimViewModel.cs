using System;
using System.ComponentModel.DataAnnotations;

namespace MaturityBenefitProc.Models.ViewModels
{
    public class ClaimViewModel
    {
        [Required(ErrorMessage = "Policy number is required")]
        [StringLength(20)]
        [Display(Name = "Policy Number")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Claimant Name")]
        public string ClaimantName { get; set; }

        [Display(Name = "Sum Assured")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal SumAssured { get; set; }

        [Display(Name = "Total Bonus")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal BonusAmount { get; set; }

        [Display(Name = "TDS Deducted")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TdsAmount { get; set; }

        [Display(Name = "Net Payable")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NetPayable { get; set; }

        [Required(ErrorMessage = "Payment mode is required")]
        [Display(Name = "Payment Mode")]
        public PaymentMode PaymentMode { get; set; }

        [Display(Name = "Bank Account Number")]
        [StringLength(20)]
        public string BankAccountNumber { get; set; }

        [Display(Name = "IFSC Code")]
        [StringLength(11)]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC format")]
        public string IfscCode { get; set; }

        [Display(Name = "Bank Name")]
        [StringLength(100)]
        public string BankName { get; set; }
    }
}
