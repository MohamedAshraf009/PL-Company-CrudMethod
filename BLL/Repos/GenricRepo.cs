using BLL.Interfaces;
using DAL.ContextDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repos
{
    public class GenricRepo<T> : IGenricRepo<T> where T : class
    {
        private readonly AppDbContexts context;

        public GenricRepo(AppDbContexts _context) 
        {
            context = _context;
        }

        public async Task<int> AddEntity(T Tentity)
        {
            await context.Set<T>().AddAsync(Tentity);
            return await  context.SaveChangesAsync();
            
        }

        public async Task<int> DeleteEntity(T Tentity)
        {
            context.Set<T>().Remove(Tentity);
           return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int? id)
        {
            
            var entity = await context.Set<T>().FindAsync(id);
            return entity;
            
        }

        public async Task<int> UpdateEntity(T Tentity)
        {
            context.Set<T>().Update(Tentity);
            return await context.SaveChangesAsync();

        }
    }
}
