using System.Collections.Generic;
using System.Linq;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;

namespace ProyectoTicketTurno.Data.Repositories
{
    public class MunicipioRepository : BaseRepository<Municipio>, IMunicipioRepository
    {
        public MunicipioRepository(AplicacionDbContext context) : base(context)
        {
        }

        public Municipio ObtenerPorNombre(string nombre)
        {
            return ObtenerPor(m => m.Nombre == nombre).FirstOrDefault();
        }

        public IEnumerable<Municipio> ObtenerMunicipiosConSolicitudes()
        {
            // Obtener municipios que tienen al menos una solicitud
            var municipiosConSolicitudes = _dbSet
                .AsNoTracking()
                .Where(m => m.IdMunicipio > 0)
                .ToList();

            return municipiosConSolicitudes;
        }
    }
}
