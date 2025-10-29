using Microsoft.EntityFrameworkCore;
using Quizera.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.BLL
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdWithIncludeAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
    }

    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                // Log exception here (example: _logger.LogError(ex, "Error in GetAllAsync"));
                Console.WriteLine($"[Error - GetAllAsync] {ex.Message}");
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                return _repository.GetAllWithIncludeAsync(includes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error - GetAllWithIncludeAsync] {ex.Message}");
                return new List<T>();
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error - GetByIdAsync] {ex.Message}");
                return null;
            }
        }

        public async Task<T> GetByIdWithIncludeAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                return await _repository.GetByIdWithIncludeAsync(id, includes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error - GetByIdWithIncludeAsync] {ex.Message}");
                return null;
            }
        }

        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                return await _repository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error - CreateAsync] {ex.Message}");
                return null;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error - UpdateAsync] {ex.Message}");
                return null;
            }
        }

        public async Task<T> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error - DeleteAsync] {ex.Message}");
                return null;
            }
        }
    }

}
