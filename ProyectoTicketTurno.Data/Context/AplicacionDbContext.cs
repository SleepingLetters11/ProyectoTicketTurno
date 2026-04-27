using System.Data.Entity;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Data.Context
{
    public class AplicacionDbContext : DbContext
    {
        public AplicacionDbContext() : base("name=AplicacionDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<NivelEducativo> NivelesEducativos { get; set; }
        public DbSet<SolicitudTurno> SolicitudesTurno { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Estudiante
            modelBuilder.Entity<Estudiante>()
                .HasKey(e => e.CURP);

            modelBuilder.Entity<Estudiante>()
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
                .HasMaxLength(1)
                .IsRequired();

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.TelefonoContacto)
                .HasMaxLength(20);

            // Índices para consultas frecuentes
            modelBuilder.Entity<Estudiante>()
                .HasIndex(e => e.Nombre);

            // Configuración de Municipio
            modelBuilder.Entity<Municipio>()
                .HasKey(m => m.IdMunicipio);

            modelBuilder.Entity<Municipio>()
                .Property(m => m.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            // Configuración de Estado
            modelBuilder.Entity<Estado>()
                .HasKey(e => e.IdEstado);

            modelBuilder.Entity<Estado>()
                .Property(e => e.NombreEstado)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Estado>()
                .Property(e => e.Abreviatura)
                .HasMaxLength(2)
                .IsRequired();

            // Configuración de NivelEducativo
            modelBuilder.Entity<NivelEducativo>()
                .HasKey(n => n.IdNivel);

            modelBuilder.Entity<NivelEducativo>()
                .Property(n => n.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            // Configuración de SolicitudTurno
            modelBuilder.Entity<SolicitudTurno>()
                .HasKey(s => s.NumeroTurno);

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.CURP)
                .HasMaxLength(18)
                .IsRequired();

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.Asunto)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.PersonaTramite)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.Parentesco)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<SolicitudTurno>()
                .Property(s => s.Estatus)
                .HasMaxLength(20)
                .IsRequired();

            // Índices para consultas frecuentes
            modelBuilder.Entity<SolicitudTurno>()
                .HasIndex(s => s.CURP);

            modelBuilder.Entity<SolicitudTurno>()
                .HasIndex(s => s.IdMunicipio);

            // Relaciones
            modelBuilder.Entity<Estudiante>()
                .HasRequired(e => e.EstadoNacimiento)
                .WithMany()
                .HasForeignKey(e => e.IdEstadoNacimiento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Estudiante>()
                .HasRequired(e => e.MunicipioEstudio)
                .WithMany()
                .HasForeignKey(e => e.IdMunicipioEstudio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Estudiante>()
                .HasRequired(e => e.NivelEducativo)
                .WithMany()
                .HasForeignKey(e => e.IdNivelEducativo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SolicitudTurno>()
                .HasRequired(s => s.Estudiante)
                .WithMany()
                .HasForeignKey(s => s.CURP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SolicitudTurno>()
                .HasRequired(s => s.Municipio)
                .WithMany()
                .HasForeignKey(s => s.IdMunicipio)
                .WillCascadeOnDelete(false);
        }
    }
}
