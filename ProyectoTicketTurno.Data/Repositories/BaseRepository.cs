using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ProyectoTicketTurno.Data.Context;

namespace ProyectoTicketTurno.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly AplicacionDbContext _context;

        public BaseRepository(AplicacionDbContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<T> ObtenerTodos()
        {
            try
            {
                return _context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener todos los registros de {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual T ObtenerPorId(object id)
        {
            try
            {
                return _context.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener registro de {typeof(T).Name} por ID: {ex.Message}", ex);
            }
        }

        public virtual IEnumerable<T> ObtenerPor(Expression<Func<T, bool>> predicado)
        {
            try
            {
                return _context.Set<T>().Where(predicado).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener registros de {typeof(T).Name} con predicado: {ex.Message}", ex);
            }
        }

        public virtual void Agregar(T entidad)
        {
            try
            {
                _context.Set<T>().Add(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar registro a {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual void Actualizar(T entidad)
        {
            try
            {
                _context.Entry(entidad).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar registro en {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual void Eliminar(T entidad)
        {
            try
            {
                _context.Set<T>().Remove(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar registro de {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual void Guardar()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar cambios: {ex.Message}", ex);
            }
        }
    }
}