using System.ComponentModel.DataAnnotations;
using Application;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/recetas")]
public class RecetasController(RecetaService servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RecetaRespuestaDto>>> Listar(
        CancellationToken ct, [StringLength(100)] string? search = null,
        [Range(1, int.MaxValue)] int? categoriaId = null,
        [Range(1, 1000000)] int page = 1, [Range(1, 100)] int pageSize = 10)
        => Ok(await servicio.ListarAsync(search, categoriaId, page, pageSize, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RecetaRespuestaDto>> Obtener(int id, CancellationToken ct)
        => await servicio.BuscarAsync(id, ct) is { } receta ? Ok(receta) : NotFound();

    [HttpPost]
    public async Task<ActionResult<RecetaRespuestaDto>> Crear(CrearRecetaDto dto, CancellationToken ct)
    {
        var receta = await servicio.CrearAsync(dto, ct);
        return CreatedAtAction(nameof(Obtener), new { id = receta.Id }, receta);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarRecetaDto dto, CancellationToken ct)
        => await servicio.ActualizarAsync(id, dto, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        => await servicio.EliminarAsync(id, ct) ? NoContent() : NotFound();
}