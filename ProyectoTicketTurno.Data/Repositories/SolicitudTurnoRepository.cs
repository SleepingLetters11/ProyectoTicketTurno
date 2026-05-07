using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoTicketTurno.Data.Repositories
{
    public class SolicitudTurnoRepository : BaseRepository<SolicitudTurno>, ISolicitudTurnoRepository
    {
        public SolicitudTurnoRepository(AplicacionDbContext context) : base(context)
        {
        }

        public SolicitudTurno ObtenerPorNumeroTurno(int numeroTurno)
        {
            return ObtenerPorId(numeroTurno);
        }

        public IEnumerable<SolicitudTurno> ObtenerPorMunicipio(string municipio)
        {
            return ObtenerPor(s => s.Municipio == municipio);
        }

        public IEnumerable<SolicitudTurno> ObtenerPorCURP(string curp)
        {
            return ObtenerPor(s => s.CURP == curp);
        }

        public IEnumerable<SolicitudTurno> ObtenerPorEstatus(EstatusEnum estatus)
        {
            return ObtenerPor(s => s.Estatus == estatus);
        }
    }
}