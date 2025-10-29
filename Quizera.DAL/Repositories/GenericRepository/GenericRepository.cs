using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.DAL
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T>? GetAllAsync();
        IQueryable<T>? GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includes); //Eager Loading
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdWithIncludeAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T?> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
    }


    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Dependence Injection

        private readonly QuizeraDB _context;

        public GenericRepository(QuizeraDB context)
        {
            _context = context;
        }

        #endregion

        public async Task<T?> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            var numerOfRowAffected = await _context.SaveChangesAsync();
            return numerOfRowAffected > 0 ? entity : null;

        }

        public IQueryable<T>? GetAllAsync()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T>? GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includes)
        {
            /// <summary>
            /// But calling .AsQueryable() makes it explicit that you’re treating it as a query, not as a list in memory.
            ///Benefit: you can build up LINQ queries dynamically:
            /// </summary>
            var query = _context.Set<T>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(model => EF.Property<int>(model, "Id") == id);
            return entity!;
        }

        public async Task<T> GetByIdWithIncludeAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var entity = await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            return entity!;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            return entity;
        }
    }
}
