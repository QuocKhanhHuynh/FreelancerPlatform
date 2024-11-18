using FreelancerPlatform.Application.Abstraction.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Infratructure.Entityframework.Implementions.Repository.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> entities;
        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await entities.AddAsync(entity);
        }
        
        public void Delete(T entity)
        {
            entities.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await entities.FindAsync(id);
        }


        public void Update(T entity)
        {
            entities.Update(entity);
        }

    }
}
