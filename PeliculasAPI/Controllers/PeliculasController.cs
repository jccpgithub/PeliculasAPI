//using Microsoft.AspNetCore.Components;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Helpers;
using PeliculasAPI.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
	[ApiController]
	[Route("api/peliculas")]
	public class PeliculasController : ControllerBase
	{
		private readonly PeliculasAPIContext context;
		private readonly IAlmacenadorArchivos almacenadorArchivos;
		private readonly IMapper mapper;
		private readonly string contenedor = "peliculas";
		public PeliculasController(PeliculasAPIContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
		{
			this.context = context;
			this.mapper = mapper;
			this.almacenadorArchivos = almacenadorArchivos;
		}

		[HttpGet]
		public async Task<ActionResult<PeliculasIndexDTO>> Get()
		{
			var top = 5;
			var hoy = DateTime.Today.AddYears(-1);

			var proximosEstrenos = await context.Peliculas
				.Where(x => x.FechaEstreno > hoy)
				.OrderBy(x => x.FechaEstreno)
				.Take(top)
				.ToListAsync();

			var enCines = await context.Peliculas
				.Where(x => x.EnCines)				
				.Take(top)
				.ToListAsync();

			var resultado = new PeliculasIndexDTO();
			resultado.FuturosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
			resultado.EnCines = mapper.Map<List<PeliculaDTO>>(enCines);

			return resultado;

			//var peliculas = await context.Peliculas.ToListAsync();
			//return mapper.Map<List<PeliculaDTO>>(peliculas);
		}

		[HttpGet("filtro")]

		public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] FiltroPeliculasDTO filtroPeliculasDTO)
		{
			var peliculasQueryable = context.Peliculas.AsQueryable();

			if (!string.IsNullOrEmpty(filtroPeliculasDTO.Titulo))
			{
				peliculasQueryable = peliculasQueryable.Where(x => x.Titulo.Contains(filtroPeliculasDTO.Titulo));
			}

			if (filtroPeliculasDTO.EnCines)
			{
				peliculasQueryable = peliculasQueryable.Where(x => x.EnCines);
			}

			if (filtroPeliculasDTO.ProximosEstrenos)
			{
				var hoy = DateTime.Today.AddYears(-1);
				peliculasQueryable = peliculasQueryable.Where(x => x.FechaEstreno > hoy);
			}

			if (filtroPeliculasDTO.GeneroId != 0)
			{
				peliculasQueryable = peliculasQueryable.Where(x => x.PeliculasGeneros.Select(y => y.GeneroId).Contains(filtroPeliculasDTO.GeneroId));
			}

			await HttpContext.InsertarParametrosPaginacion(peliculasQueryable, filtroPeliculasDTO.CantidadRegistrosPorPagina);

			//var peliculas = await peliculasQueryable.Paginar(filtroPeliculasDTO.Paginacion).ToListAsync();

			var peliculas = await peliculasQueryable.Paginar(filtroPeliculasDTO.Paginacion).ToListAsync();

			return mapper.Map<List<PeliculaDTO>>(peliculas);

		}

		[HttpGet("{id}", Name = "ObtenerPelicula")]
		public async Task<ActionResult<Pelicula>> Get(int id)
		{
			var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

			if (pelicula == null)
			{
				return NotFound();
			}				

			return pelicula;
		}
		
		[HttpPost]
		public async Task<ActionResult> Post([FromForm] PeliculaDTO peliculaDTO)
		{
			var pelicula = mapper.Map<Pelicula>(peliculaDTO);

			return Ok();
			/*if (peliculaDTO.Poster != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await peliculaDTO.Poster.CopyToAsync(memoryStream);
					var contenido = memoryStream.ToArray();
					var extension = Path.GetExtension(peliculaDTO.Poster.FileName);
					pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, peliculaDTO.Poster.ContentType);
				}
			}
			context.Add(pelicula);
			await context.SaveChangesAsync();
			var dto = mapper.Map<PeliculaDTO>(pelicula);
			return new CreatedAtRouteResult("ObtenerPelicula", new { id = pelicula.Id }, dto);*/

		}

		public async Task<ActionResult> Put(int id, [FromForm] PeliculaDTO peliculaDTO)
		{
			var peliculaDB = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

			if (peliculaDB == null)
			{
				return NotFound();
			}

			peliculaDB = mapper.Map(peliculaDTO, peliculaDB);

			if (peliculaDTO.Poster != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await peliculaDTO.Poster.CopyToAsync(memoryStream);
					var contenido = memoryStream.ToArray();
					var extension = Path.GetExtension(peliculaDTO.Poster.FileName);
					peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, peliculaDB.Poster, peliculaDTO.Poster.ContentType);
				}
			}
			
			await context.SaveChangesAsync();
			return NoContent();

		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var existe = await context.Peliculas.AnyAsync(x => x.Id == id);

			if (!existe)
			{
				return NotFound();
			}

			context.Remove(new Pelicula() { Id = id });
			await context.SaveChangesAsync();

			return NoContent();
		}

	}
}
