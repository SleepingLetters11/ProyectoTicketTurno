using System.Data.Entity;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Data.Context
{
    public class AplicacionDbContext : DbContext
    {
        public AplicacionDbContext() : base("name=ProyectoTicketTurnoConnection")
        {
            // Desabilitar lazy loading para mejor desempeño
            this.Configuration.LazyLoadingEnabled = false;

            // Usar proxy validation para validaciones de EF
            this.Configuration.ProxyCreationEnabled = true;

            // Usar AutoDetectChangesEnabled con cautela
            this.Configuration.AutoDetectChangesEnabled = true;
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<SolicitudTurno> SolicitudesTurno { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Estado> Estados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configurar Estudiante
            modelBuilder.Entity<Estudiante>()
                .HasKey(e => e.CURP)
                .Property(e => e.CURP)
                .HasMaxLength(18)
                .IsRequired();

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.ApellidoPaterno)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.ApellidoMaterno)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.Sexo)
                .IsRequired();

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.EstadoNacimiento)
                .HasMaxLength(50);

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.MunicipioEstudio)
                .HasMaxLength(100);

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.TelefonoContacto)
                .HasMaxLength(20);

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.NivelEducativo)
                .HasMaxLength(50);

            // Configurar SolicitudTurno
            modelBuilder.Entity<SolicitudTurno>()
                .HasKey(s => s.NumeroTurno)
                .Property(s => s.NumeroTurno)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.CURP)
                .HasMaxLength(18)
                .IsRequired();

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.Municipio)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.Asunto)
                .HasMaxLength(500);

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.PersonaTramitera)
                .HasMaxLength(100);

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.Parentesco)
                .HasMaxLength(50);

            // Configurar Municipio
            modelBuilder.Entity<Municipio>()
                .HasKey(m => m.IdMunicipio);

            modelBuilder.Entity<Municipio>()
                .Property(m => m.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            // Configurar Estado
            modelBuilder.Entity<Estado>()
                .HasKey(e => e.Clave);

            modelBuilder.Entity<Estado>()
                .Property(e => e.Clave)
                .HasMaxLength(10);

            modelBuilder.Entity<Estado>()
                .Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}