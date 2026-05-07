namespace ProyectoTicketTurno.Business.Models
{
    public class Municipio
    {
        public int IdMunicipio { get; set; } // PK
        public string Nombre { get; set; }
        public int ContadorTurnos { get; set; } // Para auto-incremento de turnos
    }
}