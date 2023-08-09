using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class LibroCreacionDTO
    {


        [PrimeraLetraMayuscula]
        public string Titulo { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
