using DS.AuthService.entities;
using Microsoft.EntityFrameworkCore;

namespace DS.AuthService.data;

public class CredentialContext : DbContext
{
    public CredentialContext(DbContextOptions<CredentialContext> options)
        : base(options)
    {
    }

    public DbSet<Credential> Credentials => Set<Credential>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Credential>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Username).IsUnique();
            entity.Property(c => c.Username).IsRequired();
            entity.Property(c => c.PasswordHash).IsRequired();
            entity.Property(c => c.Role).IsRequired();
        });
    }
}
