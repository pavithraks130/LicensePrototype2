using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.Core.DbContext;
using System.Data.Entity;

namespace LicenseServer.Core.Infrastructure
{
    public class GenericRepository<T> where T : class
    {
        private AppDbContext _context;
        private DbSet<T> _dbset;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public IEnumerable<T> GetData(Expression<Func<T, bool>> filterby = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = "")
        {
            var query = _dbset.AsQueryable();
            if (filterby != null)
                query = query.Where(filterby);

            if (!String.IsNullOrEmpty(includeProperties))
                foreach (var str in includeProperties.Split(new char[] { ',' }))
                    query = query.Include(str);

            if (orderby != null)
                return orderby(query).ToList();
            else
                return query.ToList();
        }


        public T GetById(object id)
        {
            return _dbset.Find(id);
        }


        public bool Delete(object id)
        {
            T data = _dbset.Find(id);
            T obj = Delete(data);
            return obj != null;
        }

        public T Delete(T obj)
        {
            if (_context.Entry(obj).State == EntityState.Detached)
                _dbset.Attach(obj);
            return _dbset.Remove(obj);
        }


        public T Update(T obj)
        {
            obj = _dbset.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            return obj;
        }

        public T Create(T obj)
        {
            obj = _dbset.Add(obj);
            _context.Entry(obj).State = EntityState.Added;
            return obj;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
