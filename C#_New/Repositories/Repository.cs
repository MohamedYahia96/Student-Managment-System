using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;

namespace StudentAssignmentManager.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly StudentSystemDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(StudentSystemDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}