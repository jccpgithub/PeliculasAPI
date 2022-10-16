using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
	[ApiController]
	[Route("api/generos")]
	public class GenerosController :ControllerBase
	{
		private readonly PeliculasAPIContext context;

		public GenerosController(PeliculasAPIContext context)
		{
			this.context = context;
		}

		[HttpGet]
		public async Task<ActionResult<List<Genero>>> Get()
		{
			return await context.Generos.ToListAsync();
		}

		[HttpGet("{id:int}", Name ="obtenerGenero")]
		public async Task<ActionResult<Genero>> Get(int id)
		{
			var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);

			if (genero == null)
			{
				return NotFound();
			}

			return genero;
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] Genero genero)
		{
			context.Add(genero);
			await context.SaveChangesAsync();

			return new CreatedAtRouteResult("obtenerGenero", new { id = genero.Id }, genero);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Put(int id, [FromBody] Genero genero)
		{
			if (id != genero.Id)
			{
				return BadRequest();
			}
			context.Entry(genero).State = EntityState.Modified;

			await context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var existe = await context.Generos.AnyAsync(X => X.Id == id);

			if (!existe)
			{
				return NotFound();
			}

			context.Remove(new Genero() { Id = id });

			await context.SaveChangesAsync();

			return NoContent();
		}
	}
}
