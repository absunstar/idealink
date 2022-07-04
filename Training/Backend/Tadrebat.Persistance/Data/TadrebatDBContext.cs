using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Entity.Model;
using Tadrebat.Model.Entity;

namespace Tadrebat.Persistance.Data
{
    public partial class TadrebatDbContext : DbContext
    {
        public virtual DbSet<TrainingCategory> TrainingCategory { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public TadrebatDbContext(DbContextOptions<TadrebatDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingCategory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.FkTrainingCategoryId).HasColumnName("FK_TrainingCategoryId");

                entity.Property(e => e.Name).HasMaxLength(256);
            });
            modelBuilder.Entity<Report>(entity =>
            {
                //entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                //entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ExamDate).HasColumnType("datetime");

                //entity.Property(e => e.StartDate).HasColumnType("datetime");
            });
            modelBuilder.Entity<ReportTrainingTrainee>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                //entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
