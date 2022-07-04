using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Employment.Entity.Model;

namespace Employment.Persistance.Data
{
    public partial class EmploymentDbContext : DbContext
    {
        public virtual DbSet<ReportJob> ReportJob { get; set; }
        public virtual DbSet<ReportCompany> ReportCompany { get; set; }
        public virtual DbSet<ReportJobSeeker> ReportJobSeeker { get; set; }
        public virtual DbSet<ReportJobSeekerLanguage> ReportJobSeekerLanguage { get; set; }
        public virtual DbSet<ReportJobSeekerResumeItem> ReportJobSeekerResumeItem { get; set; }
        public virtual DbSet<ReportJobApply> ReportJobApply { get; set; }
        public EmploymentDbContext(DbContextOptions<EmploymentDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportJob>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.Deadline).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReportCompany>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.Establish).HasColumnType("datetime");

            });
            modelBuilder.Entity<ReportJobSeeker>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.DOB).HasColumnType("datetime");

            });
            modelBuilder.Entity<ReportJobSeekerLanguage>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            });
            modelBuilder.Entity<ReportJobSeekerResumeItem>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.EndDate).HasColumnType("datetime");
            });
            modelBuilder.Entity<ReportJobApply>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            }); 
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
