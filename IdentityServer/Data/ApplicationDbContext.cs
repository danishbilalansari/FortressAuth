using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace IdentityServer.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<MfaRecoveryCode> RecoveryCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MfaRecoveryCode>(entity =>
        {
            // Configure primary key
            entity.HasKey(rc => rc.Id);

            // Configure the relationship with ApplicationUser
            entity.HasOne<ApplicationUser>()  // Remove rc => rc.User if not defined
                .WithMany(u => u.RecoveryCodes)
                .HasForeignKey(rc => rc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure CodeHash index (since we removed plain Code)
            entity.HasIndex(rc => rc.CodeHash).IsUnique();
            entity.Property(rc => rc.CodeHash)
                .HasMaxLength(256)
                .IsRequired();

            // Configure other properties
            entity.Property(rc => rc.IsUsed)
                .HasDefaultValue(false);

            entity.Property(rc => rc.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(rc => rc.ExpiryDate)
                .IsRequired();
        });
    }
}