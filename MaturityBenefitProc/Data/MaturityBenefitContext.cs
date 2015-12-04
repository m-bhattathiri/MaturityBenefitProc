using System.Data.Entity;
using MaturityBenefitProc.Models;

namespace MaturityBenefitProc.Data
{
    public class MaturityBenefitContext : DbContext
    {
        public MaturityBenefitContext()
            : base("name=MaturityBenefitDb")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Policy> Policies { get; set; }
        public virtual DbSet<Policyholder> Policyholders { get; set; }
        public virtual DbSet<MaturityClaim> MaturityClaims { get; set; }
        public virtual DbSet<Nominee> Nominees { get; set; }
        public virtual DbSet<DisbursementRecord> DisbursementRecords { get; set; }
        public virtual DbSet<BankDetail> BankDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>()
                .HasKey(p => p.PolicyId)
                .HasRequired(p => p.Policyholder)
                .WithMany(ph => ph.Policies)
                .HasForeignKey(p => p.PolicyholderId);

            modelBuilder.Entity<Policy>()
                .HasMany(p => p.Nominees)
                .WithRequired(n => n.Policy)
                .HasForeignKey(n => n.PolicyId);

            modelBuilder.Entity<Policy>()
                .Property(p => p.SumAssured)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Policy>()
                .Property(p => p.PremiumAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Policy>()
                .Property(p => p.MaturityAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MaturityClaim>()
                .HasKey(c => c.ClaimId)
                .HasRequired(c => c.Policy)
                .WithMany(p => p.MaturityClaims)
                .HasForeignKey(c => c.PolicyId);

            modelBuilder.Entity<MaturityClaim>()
                .Property(c => c.ClaimAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MaturityClaim>()
                .Property(c => c.TaxDeducted)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MaturityClaim>()
                .Property(c => c.NetPayable)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DisbursementRecord>()
                .HasKey(d => d.DisbursementId)
                .HasRequired(d => d.MaturityClaim)
                .WithMany(c => c.DisbursementRecords)
                .HasForeignKey(d => d.ClaimId);

            modelBuilder.Entity<DisbursementRecord>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BankDetail>()
                .HasKey(b => b.BankDetailId)
                .HasRequired(b => b.Policyholder)
                .WithMany(ph => ph.BankDetails)
                .HasForeignKey(b => b.PolicyholderId);

            modelBuilder.Entity<Nominee>()
                .HasKey(n => n.NomineeId);

            modelBuilder.Entity<Nominee>()
                .Property(n => n.SharePercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Policyholder>()
                .HasKey(ph => ph.PolicyholderId);

            modelBuilder.Entity<Policyholder>()
                .Property(ph => ph.PolicyholderId)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }
    }
}
