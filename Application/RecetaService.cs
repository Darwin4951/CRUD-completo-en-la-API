using Application.Dtos;
using Domain;

namespace Application;

public class RecetaService(IRecetaRepository repository)
{
    public async Task<List<RecetaRespuestaDto>> ListarAsync(string? search, int? categoriaId, int page, int pageSize, CancellationToken ct)
        => (await repository.ListarAsync(search?.Trim(), categoriaId, page, pageSize, ct)).ConvertAll(Mapear);

    public async Task<RecetaRespuestaDto?> BuscarAsync(int id, CancellationToken ct)
        => await repository.BuscarAsync(id, ct) is { } receta ? Mapear(receta) : null;

    public async Task<RecetaRespuestaDto> CrearAsync(CrearRecetaDto dto, CancellationToken ct)
    {
        await ValidarCategoriaAsync(dto.CategoriaId, ct);
        var receta = new Receta { Nombre = dto.Nombre.Trim(), CategoriaId = dto.CategoriaId };
        await repository.AgregarAsync(receta, ct);
        return Mapear(receta);
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarRecetaDto dto, CancellationToken ct)
    {
        var receta = await repository.BuscarAsync(id, ct);
        if (receta is null)
            return false;

        await ValidarCategoriaAsync(dto.CategoriaId, ct);
        receta.Nombre = dto.Nombre.Trim();
        receta.CategoriaId = dto.CategoriaId;
        await repository.GuardarAsync(ct);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken ct)
    {
        var receta = await repository.BuscarAsync(id, ct);
        if (receta is null)
            return false;

        await repository.EliminarAsync(receta, ct);
        return true;
    }

    private async Task ValidarCategoriaAsync(int id, CancellationToken ct)
    {
        if (!await repository.CategoriaExisteAsync(id, ct))
            throw new CategoriaRecetaInvalidaException();
    }

    private static RecetaRespuestaDto Mapear(Receta receta) => new(receta.Id, receta.Nombre, receta.CategoriaId);
}

public sealed class CategoriaRecetaInvalidaException : Exception;