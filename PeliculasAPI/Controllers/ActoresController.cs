using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
	[Route("api/actores")]
	[ApiController]
	public class ActoresController: ControllerBase
	{
		private readonly PeliculasAPIContext context;
		private readonly IMapper mapper;
		private readonly IAlmacenadorArchivos almacenadorArchivos;

		public ActoresController(PeliculasAPIContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
		{
			this.context = context;
			this.mapper = mapper;
			this.almacenadorArchivos = almacenadorArchivos;			
		}

		[HttpGet]
		public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery]PaginacionDTO paginacionDTO)
		{
			var queryable = context.Actores.AsQueryable();
			await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina); 
			var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
			return mapper.Map<List<ActorDTO>>(entidades);

		}

		[HttpGet("{id}", Name = "obtenerActor")]
		public async Task<ActionResult<ActorDTO>> Get(int id)
		{
			var entidad = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

			if (entidad == null)
			{
				return NotFound();
			}

			return mapper.Map<ActorDTO>(entidad);
		}
		
		[HttpPost]
		public async Task<ActionResult> Post([FromForm]ActorDTO actorDTO)
		{
			var entidad = mapper.Map<Actor>(actorDTO);
			var contenedor = "actores";

			if (actorDTO.Foto != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await actorDTO.Foto.CopyToAsync(memoryStream);
					var contenido = memoryStream.ToArray();
					var extension = Path.GetExtension(actorDTO.Foto.FileName);
					entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, actorDTO.Foto.ContentType);
				}
			}

			context.Add(entidad);
			await context.SaveChangesAsync();
			var dto = mapper.Map<ActorDTO>(entidad);
			return new CreatedAtRouteResult("obtenerActor", new { id = entidad.Id }, dto);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Put(int id, [FromForm] ActorDTO actorDTO)
		{
			var entidad = mapper.Map<Actor>(actorDTO);
			entidad.Id = id;
			context.Entry(actorDTO).State = EntityState.Modified;
			await context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var existe = await context.Actores.AnyAsync(x => x.Id == id);

			if (!existe)
			{
				return NotFound();
			}

			context.Remove(new Actor() { Id = id });
			await context.SaveChangesAsync();

			return NoContent();
		}
	}
}
