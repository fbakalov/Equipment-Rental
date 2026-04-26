using EquipmentRental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRental.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EquipmentItem> EquipmentItems { get; set; } = null!;
        public DbSet<RentalRequest> RentalRequests { get; set; } = null!;
        public DbSet<RentalRequestItem> RentalRequestItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EquipmentItem>(b =>
            {
                b.Property(e => e.Name).HasMaxLength(64).IsRequired();
                b.Property(e => e.Description).HasMaxLength(255).IsRequired();
                b.Property(e => e.ImageUrl).IsRequired();
                b.Property(e => e.Condition).IsRequired();
            });

            builder.Entity<RentalRequest>(b =>
            {
                b.Property(r => r.Status).IsRequired();
                b.HasOne(r => r.User).WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<RentalRequestItem>(b =>
            {
                b.HasOne(ri => ri.RentalRequest)
                    .WithMany(r => r.RentalRequestItems)
                    .HasForeignKey(ri => ri.RentalRequestId);

                b.HasOne(ri => ri.EquipmentItem)
                    .WithMany(e => e.RentalRequestItems)
                    .HasForeignKey(ri => ri.EquipmentItemId);
            });
        }
    }
}
