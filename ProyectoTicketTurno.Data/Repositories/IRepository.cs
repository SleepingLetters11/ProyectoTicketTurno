using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProyectoTicketTurno.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> ObtenerTodos();
        T ObtenerPorId(object id);
        IEnumerable<T> ObtenerPor(Expression<Func<T, bool>> predicado);
        void Agregar(T entidad);
        void Actualizar(T entidad);
        void Eliminar(T entidad);
        void Guardar();
    }
}
