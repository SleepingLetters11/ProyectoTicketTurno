using System;

namespace ProyectoTicketTurno.Business.Models
{
    public class Estudiante
    {
        public string CURP { get; set; } // PK
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public char Sexo { get; set; } // H/M
        public int Edad { get; set; }
        public string EstadoNacimiento { get; set; }
        public string MunicipioEstudio { get; set; }
        public string TelefonoContacto { get; set; }
        public string NivelEducativo { get; set; }
        public int Grado { get; set; }
    }
}