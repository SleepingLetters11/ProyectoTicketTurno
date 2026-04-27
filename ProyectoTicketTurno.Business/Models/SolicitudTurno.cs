using System;

namespace ProyectoTicketTurno.Business.Models
{
    public class SolicitudTurno
    {
        public int NumeroTurno { get; set; }
        public string CURP { get; set; }
        public int IdMunicipio { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Asunto { get; set; }
        public string PersonaTramite { get; set; }
        public string Parentesco { get; set; }
        public string Estatus { get; set; }
        public DateTime? FechaResolucion { get; set; }

        // Propiedades de Navegación
        public virtual Estudiante Estudiante { get; set; }
        public virtual Municipio Municipio { get; set; }

        public SolicitudTurno()
        {
            FechaSolicitud = DateTime.Now;
            Estatus = "Pendiente";
        }
    }
}
