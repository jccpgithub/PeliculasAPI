using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using PeliculasAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeliculasAPI.Tests
{
    public class BasePruebas
    {
		protected PeliculasAPIContext ConstruirContext(string nombreDB)
		{
			var opciones = new DbContextOptionsBuilder<PeliculasAPIContext>().UseInMemoryDatabase(nombreDB).Options;
			var dbContext = new PeliculasAPIContext(opciones);
			return dbContext;			
		}

		protected IMapper ConfigurarAutoMapper()
		{
			var config = new MapperConfiguration(options =>
			{
				var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
			options.AddProfile(new AutoMapperProfiles(geometryFactory));
			});

			return config.CreateMapper();
		}
    }
}
