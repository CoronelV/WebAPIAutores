namespace WebAPIAutores.DTOs
{
    public class LIbroObtencionDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public List<ComentarioObtencionDTO> Comentarios { get; set; }   
    }
}
