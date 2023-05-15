using FlowerShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowerShop.DataAccess
{
    public class FlowerContext : IdentityDbContext<ApplicationUser>
    {
        public FlowerContext(DbContextOptions<FlowerContext> options) : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
            base.OnModelCreating(builder);
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,4)");
        }
    }

    internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(60);
            builder.Property(u => u.LastName).HasMaxLength(60);
            builder.Property(u => u.Address).HasMaxLength(256);
        }
    }
}
