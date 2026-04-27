using System.Collections.Generic;
using System.Linq;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;

namespace ProyectoTicketTurno.Data.Repositories
{
    public class SolicitudTurnoRepository : Repository<SolicitudTurno>, ISolicitudTurnoRepository
    {
        public SolicitudTurnoRepository(AplicacionDbContext context) : base(context)
        {
        }

        public SolicitudTurno ObtenerPorNumeroTurno(int numeroTurno)
        {
            return _dbSet.AsNoTracking()
                .FirstOrDefault(s => s.NumeroTurno == numeroTurno);
        }

        public IEnumerable<SolicitudTurno> ObtenerPorMunicipio(int idMunicipio)
        {
            return _dbSet.AsNoTracking()
                .Where(s => s.IdMunicipio == idMunicipio)
                .ToList();
        }

        public IEnumerable<SolicitudTurno> ObtenerPorEstatus(string estatus)
        {
            return _dbSet.AsNoTracking()
                .Where(s => s.Estatus == estatus)
                .ToList();
        }
    }
}
