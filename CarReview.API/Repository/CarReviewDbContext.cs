using Microsoft.EntityFrameworkCore;
using CarReview.API.Models;
using CarReview.API.Auth.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarReview.API.Repository
{
    public partial class CarReviewDbContext : IdentityDbContext<CarReviewUser>
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
        public virtual DbSet<CarReviewUser> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=GUSTASPC\\SQLEXPRESS;Initial Catalog=reviewdb2;Integrated Security=True");
            }
        }
    }
}
