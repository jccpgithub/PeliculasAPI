using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace PeliculasAPI.Migrations
{
    public partial class UbicacionSalaDeCine2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SalasDeCine",
                columns: new[] { "Id", "Nombre", "Ubicacion" },
                values: new object[,]
                {
                    { 6, "Plaza Mayor", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-4.480482 36.655558)") },
                    { 7, "El Corte Inglés", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-4.428851 36.717208)") },
                    { 8, "Montilla", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-4.636098 37.587332)") },
                    { 9, "Camp Nou", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (2.12304 41.380972)") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SalasDeCine",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SalasDeCine",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "SalasDeCine",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SalasDeCine",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
