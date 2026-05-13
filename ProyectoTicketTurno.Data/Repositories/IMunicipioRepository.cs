using System.Collections.Generic;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Data.Repositories
{
    public interface IMunicipioRepository : IRepository<Municipio>
    {
        /// <summary>
        /// Obtiene un municipio por su nombre
        /// </summary>
        Municipio ObtenerPorNombre(string nombre);

        /// <summary>
        /// Obtiene todos los municipios que tienen solicitudes registradas
        /// </summary>
        IEnumerable<Municipio> ObtenerMunicipiosConSolicitudes();
    }
}
