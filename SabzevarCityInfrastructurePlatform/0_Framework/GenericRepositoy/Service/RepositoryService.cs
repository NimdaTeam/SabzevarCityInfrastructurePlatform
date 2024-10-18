using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.GenericRepositoy.Interface;
using Microsoft.EntityFrameworkCore;

namespace _0_Framework.GenericRepositoy.Service
{
    public class RepositoryService<TKey , T> : IRepository<TKey , T> where T : class
    {
        private readonly DbContext _context;

        public RepositoryService(DbContext context)
        {
            _context = context;
        }

        public T Get(TKey id)
        {
            return _context.Find<T>(id);
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> GetAllByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).ToList();
        }

        public void Create(T entity)
        {
            _context.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public void Delete(TKey id)
        {
            var entity = Get(id);

            Delete(entity);
        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
