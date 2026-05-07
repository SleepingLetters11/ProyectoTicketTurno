using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Infrastructure.Reports
{
    public interface IReporte
    {
        void Generar(SolicitudTurno solicitud, Estudiante estudiante);
    }
}