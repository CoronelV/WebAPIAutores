using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required]
        [StringLength(maximumLength:120, ErrorMessage ="nombre demasiado largo")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
