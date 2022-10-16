using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CarReview.API.Models;

namespace CarReview.API.Repository
{
    public partial class CarReviewDbContext : DbContext
    {
        public CarReviewDbContext()
        {
        }

        public CarReviewDbContext(DbContextOptions<CarReviewDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<Response> Responses { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=GUSTASPC\\SQLEXPRESS;Initial Catalog=reviewdb;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Response>(entity =>
            {
                entity.HasOne(d => d.FkReview)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.FkReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Responses_Reviews");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Responses_Users");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasOne(d => d.FkCar)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.FkCarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reviews_Cars");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reviews_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
