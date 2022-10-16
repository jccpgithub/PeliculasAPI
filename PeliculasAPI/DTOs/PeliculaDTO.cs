using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Helpers;
using PeliculasAPI.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
	public class PeliculaDTO
	{
		public int Id { get; set; }
		[Required]
		[StringLength(300)]
		public string Titulo { get; set; }
		public bool EnCines { get; set; }
		public DateTime FechaEstreno { get; set; }
		[PesoArchivoValidacion(PesoMaximoEnMegabytes:4) ]
		[TipoArchivoValidacion(tiposValidos: new string[] { "image/jpeg", "image/png", "image/gif" })]
		public IFormFile Poster { get; set; }

		[ModelBinder(BinderType =typeof(TypeBinder<List<int>>))]
		public List<int> GenerosIDs { get; set; }

		[ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculasDTO>>))]
		public List<ActorPeliculasDTO> Actores { get; set; }

	}
}
