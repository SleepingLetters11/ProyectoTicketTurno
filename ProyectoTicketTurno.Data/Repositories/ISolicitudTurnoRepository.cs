using ProyectoTicketTurno.Business.Models;
using System.Collections.Generic;

namespace ProyectoTicketTurno.Data.Repositories
{
    public interface ISolicitudTurnoRepository : IRepository<SolicitudTurno>
    {
        SolicitudTurno ObtenerPorNumeroTurno(int numeroTurno);
        IEnumerable<SolicitudTurno> ObtenerPorMunicipio(string municipio);
        IEnumerable<SolicitudTurno> ObtenerPorCURP(string curp);
        IEnumerable<SolicitudTurno> ObtenerPorEstatus(EstatusEnum estatus);
    }
}