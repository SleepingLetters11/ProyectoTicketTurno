using System;

namespace ProyectoTicketTurno.Business.Models
{
    public class SolicitudTurno
    {
        public int NumeroTurno { get; set; } // PK
        public string CURP { get; set; } // FK
        public string Municipio { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Asunto { get; set; }
        public string PersonaTramitera { get; set; }
        public string Parentesco { get; set; }
        public EstatusEnum Estatus { get; set; } // Pendiente/Resuelto
    }

    public enum EstatusEnum
    {
        Pendiente = 0,
        Resuelto = 1
    }
}