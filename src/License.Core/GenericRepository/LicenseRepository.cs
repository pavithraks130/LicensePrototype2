using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using License.Core.DBContext;

namespace License.Core.GenericRepository
{
    public class LicenseRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public LicenseRepository(ApplicationDbContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public IEnumerable<T> GetData(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);
            foreach (string str in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(str);
            if (orderby != null)
                return orderby(query).ToList();
            return query.ToList();
        }

        //If Key is String
        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public bool Delete(object id)
        {
            T obj = _dbSet.Find(id);
            T data = Delete(obj);
            return data != null;
        }

        public T Delete(T obj)
        {
            if (_context.Entry(obj).State == EntityState.Detached)
                _dbSet.Attach(obj);
            return _dbSet.Remove(obj);
        }

        public T Update(T obj)
        {
            obj = _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            return obj;
        }

        public T Create(T obj)
        {
            obj = _dbSet.Add(obj);
            _context.Entry(obj).State = EntityState.Added;
            return obj;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
