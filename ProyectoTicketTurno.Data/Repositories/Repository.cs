using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ProyectoTicketTurno.Data.Context;

namespace ProyectoTicketTurno.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AplicacionDbContext _context;
        protected DbSet<T> _dbSet;

        public Repository(AplicacionDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> ObtenerTodos()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public virtual T ObtenerPorId(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual IEnumerable<T> ObtenerPor(Expression<Func<T, bool>> predicado)
        {
            return _dbSet.AsNoTracking().Where(predicado).ToList();
        }

        public virtual void Agregar(T entidad)
        {
            _dbSet.Add(entidad);
        }

        public virtual void Actualizar(T entidad)
        {
            _dbSet.Attach(entidad);
            _context.Entry(entidad).State = EntityState.Modified;
        }

        public virtual void Eliminar(T entidad)
        {
            if (_context.Entry(entidad).State == EntityState.Detached)
            {
                _dbSet.Attach(entidad);
            }
            _dbSet.Remove(entidad);
        }

        public virtual void Guardar()
        {
            _context.SaveChanges();
        }
    }
}
