using Domain;

namespace Application;

public interface IRecetaRepository
{
    Task<List<Receta>> ListarAsync(string? search, int? categoriaId, int pagina, int tamano, CancellationToken ct);
    Task<Receta?> BuscarAsync(int id, CancellationToken ct);
    Task<bool> CategoriaExisteAsync(int id, CancellationToken ct);
    Task AgregarAsync(Receta receta, CancellationToken ct);
    Task GuardarAsync(CancellationToken ct);
    Task EliminarAsync(Receta receta, CancellationToken ct);
}

public sealed class NombreRecetaDuplicadoException : Exception;