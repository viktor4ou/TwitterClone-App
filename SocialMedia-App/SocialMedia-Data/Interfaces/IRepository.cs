using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public List<T> GetAll();
        public List<T> GetAllBy(Expression<Func<T, bool>> expression);
        public T GetBy(Expression<Func<T,bool>> expression);
        public void Add(T entity);
        public void Remove(T entity);
        public void Save();
    }
}
