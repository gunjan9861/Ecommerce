using Microsoft.EntityFrameworkCore;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _context.Products.Include(x => x.Category);
            _dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
           _dbset.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _dbset.RemoveRange(entity);
        }

        public IEnumerable<T> GetAll(string? includePropties=null)
        {
            IQueryable<T> query = _dbset;
            if(includePropties != null)
            {
                foreach(var property in includePropties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query  = query.Include(property);
                }
            }
          return query.ToList();
        }

        public T GetT(Expression<Func<T, bool>> predicate, string? includePropties = null)
        {
            IQueryable<T> query = _dbset;
            query = query.Where(predicate);
            if (includePropties != null)
            {
                foreach (var property in includePropties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }
    }
}
