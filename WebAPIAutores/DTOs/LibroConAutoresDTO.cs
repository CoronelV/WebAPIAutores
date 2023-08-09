namespace WebAPIAutores.DTOs
{
    public class LibroConAutoresDTO :LIbroObtencionDTO
    {
        public List<AutorObtencionDTO> Autores { get; set; }

    }
}
