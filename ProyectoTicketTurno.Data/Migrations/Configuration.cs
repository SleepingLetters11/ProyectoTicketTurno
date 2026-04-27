using System.Data.Entity.Migrations;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;

namespace ProyectoTicketTurno.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AplicacionDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AplicacionDbContext context)
        {
            base.Seed(context);

            // Datos iniciales de Estados
            if (!context.Estados.Any())
            {
                var estados = new[]
                {
                    new Estado { NombreEstado = "Coahuila de Zaragoza", Abreviatura = "CO" },
                    new Estado { NombreEstado = "Chihuahua", Abreviatura = "CH" },
                    new Estado { NombreEstado = "Durango", Abreviatura = "DG" },
                    new Estado { NombreEstado = "Nuevo León", Abreviatura = "NL" },
                    new Estado { NombreEstado = "Tamaulipas", Abreviatura = "TM" }
                };

                foreach (var estado in estados)
                {
                    context.Estados.Add(estado);
                }

                context.SaveChanges();
            }

            // Datos iniciales de Municipios
            if (!context.Municipios.Any())
            {
                var municipios = new[]
                {
                    new Municipio { Nombre = "Saltillo", ContadorTurno = 0 },
                    new Municipio { Nombre = "Torreón", ContadorTurno = 0 },
                    new Municipio { Nombre = "Monclova", ContadorTurno = 0 },
                    new Municipio { Nombre = "Parras de la Fuente", ContadorTurno = 0 },
                    new Municipio { Nombre = "Arteaga", ContadorTurno = 0 }
                };

                foreach (var municipio in municipios)
                {
                    context.Municipios.Add(municipio);
                }

                context.SaveChanges();
            }

            // Datos iniciales de Niveles Educativos
            if (!context.NivelesEducativos.Any())
            {
                var niveles = new[]
                {
                    new NivelEducativo { Nombre = "Preescolar" },
                    new NivelEducativo { Nombre = "Primaria" },
                    new NivelEducativo { Nombre = "Secundaria" },
                    new NivelEducativo { Nombre = "Preparatoria" }
                };

                foreach (var nivel in niveles)
                {
                    context.NivelesEducativos.Add(nivel);
                }

                context.SaveChanges();
            }
        }
    }
}
