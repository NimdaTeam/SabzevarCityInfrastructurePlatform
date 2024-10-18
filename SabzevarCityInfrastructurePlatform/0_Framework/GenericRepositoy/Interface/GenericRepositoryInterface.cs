using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _0_Framework.GenericRepositoy.Interface
{
    public interface IRepository <TKey , T> where T :class
    {
        T Get(TKey id);

        List<T> GetAll();

        List<T> GetAllByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(TKey id);

        bool Exists(Expression<Func<T, bool>> expression);

        void SaveChanges();
    }
}
