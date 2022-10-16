using Microsoft.AspNetCore.Http;
using PeliculasAPI.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
	public class ActorDTO
	{		
		[Required]
		[StringLength(120)]
		public string Nombre { get; set; }
		public DateTime FechaNacimiento { get; set; }
		[PesoArchivoValidacion(PesoMaximoEnMegabytes: 4)]
		[TipoArchivoValidacion(tiposValidos: new string[] { "image/jpeg", "image/png", "image/gif"})]
		public IFormFile Foto { get; set; }
	}
}
