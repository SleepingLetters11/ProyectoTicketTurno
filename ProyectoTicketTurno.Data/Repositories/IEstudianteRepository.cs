using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Data.Repositories
{
    public interface IEstudianteRepository : IRepository<Estudiante>
    {
        Estudiante ObtenerPorCURP(string curp);
    }
}