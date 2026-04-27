using System.Linq;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;

namespace ProyectoTicketTurno.Data.Repositories
{
    public class EstudianteRepository : Repository<Estudiante>, IEstudianteRepository
    {
        public EstudianteRepository(AplicacionDbContext context) : base(context)
        {
        }

        public Estudiante ObtenerPorCURP(string curp)
        {
            return _dbSet.AsNoTracking()
                .FirstOrDefault(e => e.CURP == curp);
        }
    }
}
