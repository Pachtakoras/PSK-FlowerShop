using FlowerShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace FlowerShop.DataAccess
{
    public class FlowerContext : IdentityDbContext<ApplicationUser>
    {
        public FlowerContext(DbContextOptions<FlowerContext> options) : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {


            base.OnModelCreating(builder);
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,4)");

            builder.Entity<Order>()
                .Property(p => p.PriceTotal)
                .HasColumnType("decimal(18,4)");

            builder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });


            builder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);
            builder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.ProductId);
            builder.Entity<OrderProduct>()
                .Property(op => op.Quantity)
                .IsRequired();
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
