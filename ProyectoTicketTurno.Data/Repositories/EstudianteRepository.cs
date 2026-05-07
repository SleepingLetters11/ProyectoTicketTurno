using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;

namespace ProyectoTicketTurno.Data.Repositories
{
    public class EstudianteRepository : BaseRepository<Estudiante>, IEstudianteRepository
    {
        public EstudianteRepository(AplicacionDbContext context) : base(context)
        {
        }

        public Estudiante ObtenerPorCURP(string curp)
        {
            return ObtenerPorId(curp);
        }
    }
}