using ControleLicenca.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleLicenca.Api.Data;

public class SeuDbContext : DbContext
{
    public SeuDbContext(DbContextOptions<SeuDbContext> options) : base(options)
    {
    }

    public DbSet<SistemaControle> SistemaControle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SistemaControle>(entity =>
        {
            entity.HasKey(e => e.ClienteId);
            entity.Property(e => e.ClienteId).HasDefaultValueSql("NEWID()");
            entity.Property(e => e.NomeCliente).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Bloqueado).HasDefaultValue(false);
        });
    }
}
