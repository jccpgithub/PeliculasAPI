using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [Route("api/SalasDeCine")]
    [ApiController]
    public class SalasDeCinesController : ControllerBase
    {
        private readonly PeliculasAPIContext _context;
		private readonly IMapper mapper;


        public SalasDeCinesController(PeliculasAPIContext context, IMapper mapper)
        {
            _context = context;
			this.mapper = mapper;
        }

        // GET: api/SalasDeCines
        [HttpGet]
        public async Task<ActionResult<List<SalaDeCineDTO>>> Get()
        {
			var queryable = _context.SalasDeCine.AsQueryable();
			var entidades = await queryable.ToListAsync();
			return mapper.Map<List<SalaDeCineDTO>>(entidades);
			//return mapper.Map<List<SalaDeCineDTO>>(entidades);
			//return await _context.SalasDeCine..SalasDeCine.ToListAsync();
        }

        // GET: api/SalasDeCines/5
        [HttpGet("{id}", Name="obtenerSalaDeCine")]
        public async Task<ActionResult<SalaDeCine>> Get(int id)
        {
            var salaDeCine = await _context.SalasDeCine.FindAsync(id);

            if (salaDeCine == null)
            {
                return NotFound();
            }

            return salaDeCine;
        }

        // PUT: api/SalasDeCines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SalaDeCine salaDeCine)
        {
            if (id != salaDeCine.Id)
            {
                return BadRequest();
            }

            _context.Entry(salaDeCine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaDeCineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SalasDeCines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SalaDeCine>> Post([FromBody] SalaDeCineDTO salaDeCineDTO)
        {
			var entidad = mapper.Map<SalaDeCine>(salaDeCineDTO);
			//var contenedor = "salasdecine";


			_context.SalasDeCine.Add(entidad);

			await _context.SaveChangesAsync();
			var dto = mapper.Map<SalaDeCineDTO>(entidad);
			//return new CreatedAtRouteResult("obtenerActor", new { id = entidad.Id }, dto);

			//await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalaDeCine", new { id = salaDeCineDTO.Id }, salaDeCineDTO);
        }

        // DELETE: api/SalasDeCines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SalaDeCine>> Delete(int id)
        {
            var salaDeCine = await _context.SalasDeCine.FindAsync(id);
            if (salaDeCine == null)
            {
                return NotFound();
            }

            _context.SalasDeCine.Remove(salaDeCine);
            await _context.SaveChangesAsync();

            return salaDeCine;
        }

        private bool SalaDeCineExists(int id)
        {
            return _context.SalasDeCine.Any(e => e.Id == id);
        }
    }
}
