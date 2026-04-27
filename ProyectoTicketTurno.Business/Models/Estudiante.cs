using System;

namespace ProyectoTicketTurno.Business.Models
{
    public class Estudiante
    {
        public string CURP { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public char Sexo { get; set; }
        public int Edad { get; set; }
        public int IdEstadoNacimiento { get; set; }
        public int IdMunicipioEstudio { get; set; }
        public string TelefonoContacto { get; set; }
        public int IdNivelEducativo { get; set; }
        public int Grado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        // Propiedades de Navegación
        public virtual Estado EstadoNacimiento { get; set; }
        public virtual Municipio MunicipioEstudio { get; set; }
        public virtual NivelEducativo NivelEducativo { get; set; }

        public Estudiante()
        {
            FechaRegistro = DateTime.Now;
            Activo = true;
        }
    }
}
