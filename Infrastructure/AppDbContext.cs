using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Receta> Recetas => Set<Receta>();
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Receta>().Property(receta => receta.Nombre).HasMaxLength(100);
        modelBuilder.Entity<Receta>().HasIndex(receta => receta.Nombre).IsUnique();

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Entradas" },
            new Categoria { Id = 2, Nombre = "Platos fuertes" },
            new Categoria { Id = 3, Nombre = "Postres" }
        );
    }
}
