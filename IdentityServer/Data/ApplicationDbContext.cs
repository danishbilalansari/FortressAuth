using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            entity.HasIndex(rc => rc.Code).IsUnique();
            entity.HasOne(rc => rc.User)
                .WithMany(u => u.RecoveryCodes)
                .HasForeignKey(rc => rc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}