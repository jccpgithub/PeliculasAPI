using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PeliculasAPI
{
	public class PeliculasAPIContext : IdentityDbContext
	{
		public PeliculasAPIContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PeliculasActores>().HasKey(x => new { x.ActorId, x.PeliculaId });
			modelBuilder.Entity<PeliculasGeneros>().HasKey(x => new { x.GeneroId, x.PeliculaId });
			modelBuilder.Entity<PeliculasSalasDeCine>().HasKey(x => new {x.PeliculaId, x.SalaDeCineId});

			SeedData(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}

		private void SeedData(ModelBuilder modelBuilder)
		{
			//Vamos a la páina de guidgenerator.com para obtener un Guid de usuario
			var rolAdminId = "efa66892-c75b-4cc6-bef9-8f264282b1e5";
			var usuarioAdminId = "0db96fbd-47cc-4f52-8001-7173030da52d";

			var rolAdmin = new IdentityRole()
			{
				Id = rolAdminId,
				Name = "Admin",
				NormalizedName = "Admin"
			};

			var passwordHasher = new PasswordHasher<IdentityUser>();

			var username = "jccobos@outlook.com";

			var usuarioAdmin = new IdentityUser()
			{
				Id = usuarioAdminId,
				UserName = username,
				NormalizedUserName = username,
				Email = username,
				NormalizedEmail = username,
				PasswordHash = passwordHasher.HashPassword(null, "Aa123456!")
			};

			modelBuilder.Entity<IdentityUser>().HasData(usuarioAdmin);
			modelBuilder.Entity<IdentityRole>().HasData(rolAdmin);
			modelBuilder.Entity<IdentityUserClaim<string>>().
				HasData(new IdentityUserClaim<string>() {
				Id=1,
				ClaimType = ClaimTypes.Role,
				UserId = usuarioAdminId,
				ClaimValue= "Admin"})
				;

			


			var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

			modelBuilder.Entity<SalaDeCine>().HasData(new List<SalaDeCine>()
			{
				new SalaDeCine{ Id = 6, Nombre = "Plaza Mayor", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-4.480482, 36.655558))},
			new SalaDeCine { Id = 7, Nombre = "El Corte Inglés", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-4.428851, 36.717208))},
				new SalaDeCine { Id = 8, Nombre = "Montilla", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-4.636098, 37.587332))},
			new SalaDeCine { Id = 9, Nombre = "Camp Nou", Ubicacion = geometryFactory.CreatePoint(new Coordinate(2.123040, 41.380972)) }
			});
			




			var aventura = new Genero() { Id = 4, Nombre = "Aventura" };
			var animation = new Genero() { Id = 5, Nombre = "Animacion" };
			var suspense = new Genero() { Id = 6, Nombre = "Suspense" };
			var romance = new Genero() { Id = 7, Nombre = "Romance" };

			modelBuilder.Entity<Genero>().HasData(new List<Genero> { aventura, animation, suspense, romance });

			var jimCarrey = new Actor() { Id = 5, Nombre = "Jim Carrey", FechaNacimiento = new DateTime(1962,01,17) };
			var robertDowney = new Actor() { Id = 6, Nombre = "Robert Downey Jr.", FechaNacimiento = new DateTime(1965, 01, 17) };
			var chrisEvans = new Actor() { Id = 7, Nombre = "Chris Evans", FechaNacimiento = new DateTime(1981, 06, 13) };

			modelBuilder.Entity<Actor>().HasData(new List<Actor> { jimCarrey, robertDowney, chrisEvans });



			var endgame = new Pelicula()
			{
				Id = 2,
				Titulo = "Avengers: Endgame",
				EnCines = true,
				FechaEstreno = new DateTime(2019, 04, 26)
			};

			var iw = new Pelicula()
			{
				Id = 3,
				Titulo = "Avengers: Infinity Wars",
				EnCines = false,
				FechaEstreno = new DateTime(2019, 04, 26)
			};

			var sonic = new Pelicula()
			{
				Id = 4,
				Titulo = "Sonic the HedgeHog",
				EnCines = false,
				FechaEstreno = new DateTime(2020, 02, 28)
			};

			var emma = new Pelicula()
			{
				Id = 5,
				Titulo = "Emma",
				EnCines = false,
				FechaEstreno = new DateTime(2020, 02, 11)
			};

			var wonderwoman = new Pelicula()
			{
				Id = 6,
				Titulo = "Wonder Woman 1984",
				EnCines = false,
				FechaEstreno = new DateTime(2020, 08, 14)
			};

			modelBuilder.Entity<Pelicula>().HasData(new List<Pelicula>
			{
				endgame, iw, sonic, emma, wonderwoman
			});

			modelBuilder.Entity<PeliculasGeneros>().HasData(new List<PeliculasGeneros>()
			{
				new PeliculasGeneros(){PeliculaId =endgame.Id, GeneroId = suspense.Id},
				new PeliculasGeneros(){PeliculaId =endgame.Id, GeneroId = aventura.Id},
				new PeliculasGeneros(){PeliculaId =iw.Id, GeneroId = suspense.Id},
				new PeliculasGeneros(){PeliculaId =iw.Id, GeneroId = aventura.Id},
				new PeliculasGeneros(){PeliculaId =sonic.Id, GeneroId = aventura.Id},
				new PeliculasGeneros(){PeliculaId =emma.Id, GeneroId = suspense.Id},
				new PeliculasGeneros(){PeliculaId =emma.Id, GeneroId = romance.Id},
				new PeliculasGeneros(){PeliculaId =wonderwoman.Id, GeneroId = suspense.Id},
				new PeliculasGeneros(){PeliculaId =wonderwoman.Id, GeneroId = aventura.Id}
			});

			modelBuilder.Entity<PeliculasActores>().HasData(new List<PeliculasActores>()
			{
				new PeliculasActores(){PeliculaId =endgame.Id, ActorId = robertDowney.Id, Personaje="Tony Stark",Orden=0},
				new PeliculasActores(){PeliculaId =endgame.Id, ActorId = chrisEvans.Id, Personaje="Steve Rogers",Orden=1},
				new PeliculasActores(){PeliculaId =iw.Id, ActorId = robertDowney.Id, Personaje="Tony Stark",Orden=0},
				new PeliculasActores(){PeliculaId =iw.Id, ActorId = chrisEvans.Id, Personaje="Steve Rogers",Orden=1},
				new PeliculasActores(){PeliculaId =sonic.Id, ActorId = robertDowney.Id, Personaje="Dr. Ivo Robotnik",Orden=0},
				

			});










		}
		public DbSet<Genero> Generos { get; set; }
		public DbSet<Actor> Actores { get; set; }
		public DbSet<Pelicula> Peliculas { get; set; }

		public DbSet<PeliculasActores> PeliculasActores { get; set; }
		public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
		public DbSet<SalaDeCine> SalasDeCine { get; set; }
		public DbSet<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }

	}
}
