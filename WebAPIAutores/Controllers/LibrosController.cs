using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public LibrosController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name ="obtenerLibro")]
        public async Task<ActionResult<LibroConAutoresDTO>> Get(int id)
        {
            var libro_raw = await context.Libros
                .Include( libroDB => libroDB.Comentarios )
                .Include( libroDb => libroDb.AutoresLibros) //Obtengo la relacion a autor-libro
                .ThenInclude( autorLibroDb => autorLibroDb.Autor) // Me meto a autor-libro e incluyo al autor de la relación
                .FirstOrDefaultAsync(x => x.Id == id);
            if (libro_raw == null)
            {
                return NotFound();
            }
            else
            {
                var libro = new LIbroObtencionDTO
                {
                    Titulo = libro_raw.Titulo,
                    Comentarios = libro_raw.Comentarios.Select(coment => new ComentarioObtencionDTO
                    {
                        LibroId = coment.LibroId,
                        Contenido = coment.Contenido
                    }).ToList(),
                    Autores = libro_raw.AutoresLibros.Select(a => new AutorObtencionDTO
                    {

                        Id = a.Autor.Id,
                        Nombre = a.Autor.Nombre,
                    }).ToList()
                };
                //Finalmente se ordenan los autores por el orden que se estipuló
                //libro.Autores = libro.Autores.OrderBy(x => x.Orden).ToList();
                return libro;






                return mapper.Map<LibroConAutoresDTO>(libro_raw);


            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("Sin autores");
            }
           

            //Se crea un nuevo libro
            var libro = new Libro()
            {
                Titulo = libroCreacionDTO.Titulo,
                AutoresLibros = new List<AutorLibro>() //Solo se inicializa la lista
            };

            //Se obtienen los autores de cada ID
            var autores = await context.Autores.Where(a => libroCreacionDTO.AutoresIds.Contains(a.Id)).ToListAsync();

            //Se agregan los autores a la lista de autoreslibros
            foreach (var autor in autores)
            {
                libro.AutoresLibros.Add(new AutorLibro
                {
                    Autor = autor,
                    Libro = libro
                });
            }



            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDTO = mapper.Map<LIbroObtencionDTO>(libro);
            return CreatedAtRoute("obtenerLibro", new { id = libro.Id }, libroDTO);
        }
    }
}
