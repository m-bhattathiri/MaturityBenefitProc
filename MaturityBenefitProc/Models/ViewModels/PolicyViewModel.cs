using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaturityBenefitProc.Models.ViewModels
{
    public class PolicyViewModel
    {
        [Display(Name = "Policy Number")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Policyholder Name")]
        public string HolderName { get; set; }

        [Display(Name = "Policy Type")]
        public PolicyType PolicyType { get; set; }

        [Display(Name = "Status")]
        public PolicyStatus Status { get; set; }

        [Display(Name = "Sum Assured")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal SumAssured { get; set; }

        [Display(Name = "Maturity Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime MaturityDate { get; set; }

        [Display(Name = "Commencement Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime CommencementDate { get; set; }

        public List<Nominee> Nominees { get; set; }

        [Display(Name = "Premiums Paid")]
        public int PremiumsPaid { get; set; }

        [Display(Name = "Premiums Due")]
        public int PremiumsDue { get; set; }

        [Display(Name = "Premium Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal PremiumAmount { get; set; }

        [Display(Name = "Premium Frequency")]
        public PremiumFrequency PremiumFrequency { get; set; }

        public PolicyViewModel()
        {
            Nominees = new List<Nominee>();
            PremiumsPaid = 0;
            PremiumsDue = 0;
        }
    }
}
