using Application;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class RecetaRepository(AppDbContext db) : IRecetaRepository
{
    public Task<List<Receta>> ListarAsync(string? search, int? categoriaId, int pagina, int tamano, CancellationToken ct)
    {
        var consulta = db.Recetas.AsNoTracking();
        if (!string.IsNullOrEmpty(search))
            consulta = consulta.Where(receta => receta.Nombre.Contains(search));
        if (categoriaId.HasValue)
            consulta = consulta.Where(receta => receta.CategoriaId == categoriaId.Value);

        return consulta.OrderBy(receta => receta.Id).Skip((pagina - 1) * tamano).Take(tamano).ToListAsync(ct);
    }

    public Task<Receta?> BuscarAsync(int id, CancellationToken ct)
        => db.Recetas.SingleOrDefaultAsync(receta => receta.Id == id, ct);

    public Task<bool> CategoriaExisteAsync(int id, CancellationToken ct)
        => db.Categorias.AnyAsync(categoria => categoria.Id == id, ct);

    public async Task AgregarAsync(Receta receta, CancellationToken ct)
    {
        db.Recetas.Add(receta);
        await GuardarAsync(ct);
    }

    public async Task GuardarAsync(CancellationToken ct)
    {
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 } sql
            && sql.Message.Contains("IX_Recetas_Nombre", StringComparison.Ordinal))
        {
            throw new NombreRecetaDuplicadoException();
        }
    }

    public async Task EliminarAsync(Receta receta, CancellationToken ct)
    {
        db.Recetas.Remove(receta);
        await GuardarAsync(ct);
    }
}