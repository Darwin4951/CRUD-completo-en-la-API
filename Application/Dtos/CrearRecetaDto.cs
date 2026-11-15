using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class CrearRecetaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre admite hasta 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una categoría válida.")]
    public int CategoriaId { get; set; }
}