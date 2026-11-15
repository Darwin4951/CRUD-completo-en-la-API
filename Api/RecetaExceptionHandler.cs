using Application;
using Microsoft.AspNetCore.Diagnostics;

namespace Api;

public class RecetaExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, titulo) = exception switch
        {
            NombreRecetaDuplicadoException => (409, "Ya existe una receta con ese nombre."),
            CategoriaRecetaInvalidaException => (400, "La categoría indicada no existe."),
            _ => (0, string.Empty)
        };

        if (status == 0)
            return false;

        await Results.Problem(statusCode: status, title: titulo).ExecuteAsync(context);
        return true;
    }
}