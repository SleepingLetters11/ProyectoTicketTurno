namespace ProyectoTicketTurno.Business.Models
{
    public class Municipio
    {
        public int IdMunicipio { get; set; }
        public string Nombre { get; set; }
        public int ContadorTurno { get; set; }

        public int ObtenerProximoTurno()
        {
            ContadorTurno++;
            return ContadorTurno;
        }
    }
}
