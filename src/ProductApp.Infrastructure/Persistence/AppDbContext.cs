using Microsoft.EntityFrameworkCore;
using ProductApp.Data.Entities;

namespace ProductApp.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Name)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasPrecision(10, 2);

            entity.Property(x => x.Description);
        });

        base.OnModelCreating(modelBuilder);
    }
}
