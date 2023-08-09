using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System.Runtime.CompilerServices;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")] //Esto es para acceder a los comentarios de un libro
    public class ComentariosController : ControllerBase
    {
        //Conexión con la base de datos
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        
        //Constructor
        public ComentariosController(
            ApplicationDbContext context,
            IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }


        //Rutas

        [HttpGet]
        public async Task<ActionResult<List<ComentarioObtencionDTO>>> Get (int libroId)
        {
            var comentarios = await context.Comentarios
                .Where( comentarioDB => comentarioDB.LibroId == libroId)
                .Select(element => new ComentarioObtencionDTO
                {
                    Contenido = element.Contenido,
                    LibroId = element.LibroId //element.LibroId == libroId
                })
                .ToListAsync();

            if(comentarios.Count == 0 || comentarios == null)
            {
                return NotFound();
            }
            else
            {
                return comentarios;

            }
        }
        [HttpGet("{id:int}", Name = "obtenerComentarioPorId")]
        public async Task<ActionResult<ComentarioObtencionDTO>> GetPorId(int id) {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(comentarioDB => comentarioDB.Id == id);
            if(comentario == null)
            {
                return NotFound();
            }
            else
            {

                return mapper.Map<ComentarioObtencionDTO>(comentario);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }
            else
            {
                var comentario = new Comentario
                {
                    LibroId = libroId,
                    Contenido = comentarioCreacionDTO.Contenido,
                };

                context.Add(comentario);
                await context.SaveChangesAsync();
                var comentarioDTO = mapper.Map<ComentarioObtencionDTO>(comentario);
                return CreatedAtRoute("obtenerComentarioPorId", new { id = comentario.Id, libroId=comentario.LibroId }, comentarioDTO);
            }
        }
    }
}
