using System.Collections.Generic;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Data.Repositories
{
    public interface ISolicitudTurnoRepository : IRepository<SolicitudTurno>
    {
        SolicitudTurno ObtenerPorNumeroTurno(int numeroTurno);
        IEnumerable<SolicitudTurno> ObtenerPorMunicipio(int idMunicipio);
        IEnumerable<SolicitudTurno> ObtenerPorEstatus(string estatus);
    }
}
