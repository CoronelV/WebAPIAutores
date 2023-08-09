using AutoMapper;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Utilidades
{
    public class AutorMaperProfile : Profile
    {
        public AutorMaperProfile() 
        {
            CreateMap<Autor, AutorObtencionDTO>();
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorConLibrosDTO>()
                .ForMember(xc => xc.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));

            CreateMap<Libro, LIbroObtencionDTO>();
            CreateMap<Libro, LibroConAutoresDTO>()
                .ForMember(xc => xc.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));


            CreateMap<Comentario, ComentarioObtencionDTO>();
        }
        private List<LIbroObtencionDTO> MapAutorDTOLibros(Autor autor, AutorObtencionDTO autorObtencionDTO)
        {
            var resultado = new List<LIbroObtencionDTO>();
            if (autor.AutoresLibros == null) { return resultado; }

            //Lógica para meter los libros en la lista
            foreach (var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new LIbroObtencionDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });   
            }


            return resultado;
        }
        private List<AutorObtencionDTO> MapLibroDTOAutores(Libro libro, LIbroObtencionDTO libroObtencionDTO)
        {
            var resultado = new List<AutorObtencionDTO>();
            var comentarios = new List<ComentarioObtencionDTO>();
            if (libro.AutoresLibros == null) { return resultado; }
            if (libro.Comentarios == null) { return resultado; }

            //Lógica para meter los autores en la lista
            foreach (var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorObtencionDTO()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }

            return resultado;
        }

    }
}
