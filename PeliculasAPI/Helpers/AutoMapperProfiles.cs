using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Helpers
{
	public class AutoMapperProfiles:Profile
	{
		public AutoMapperProfiles(GeometryFactory geometryFactory)
		{
			CreateMap<Actor, ActorDTO>().ReverseMap();
			CreateMap<Pelicula, PeliculaDTO>().ReverseMap();

			CreateMap<IdentityUser, Usuario>();

			CreateMap<SalaDeCine, SalaDeCineDTO>()
				.ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
				.ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));


			CreateMap<SalaDeCineDTO, SalaDeCine>()
				.ForMember(x => x.Ubicacion, x => x.MapFrom(y =>
				geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

			



			CreateMap<PeliculaDTO, Pelicula>().ForMember(x => x.Poster, options => options.Ignore()).
				ForMember(x => x.PeliculasGeneros, options => options.MapFrom(MapPeliculasGeneros)).
				ForMember(x => x.PeliculasActores, options => options.MapFrom(MapPeliculasActores));



		}

		private List<PeliculasGeneros> MapPeliculasGeneros(PeliculaDTO peliculaDTO, Pelicula pelicula)
		{
			var resultado = new List<PeliculasGeneros>();
			if (peliculaDTO.GenerosIDs == null)
			{
				return resultado;
			}
			foreach(var id in peliculaDTO.GenerosIDs)
			{
				resultado.Add(new PeliculasGeneros() { GeneroId = id });
			}

			return resultado;
		}

		private List<PeliculasActores> MapPeliculasActores(PeliculaDTO peliculaDTO, Pelicula pelicula)
		{
			var resultado = new List<PeliculasActores>();
			if (peliculaDTO.Actores == null)
			{
				return resultado;
			}
			foreach (var actor in peliculaDTO.Actores)
			{
				resultado.Add(new PeliculasActores() { ActorId = actor.ActorId, Personaje = actor.Personaje });
			}

			return resultado;
		}

	}
}
