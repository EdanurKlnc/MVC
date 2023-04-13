using Microsoft.EntityFrameworkCore;
using PhoneBookDataLayer.InterfaceOfRepo;
using System.Linq.Expressions;

namespace PhoneBookDataLayer.ImplementationOfRepo
{
    public class Repository<T, Id> : IRepository<T, Id> where T : class, new()
    {
        protected readonly MyContext _context;
        public Repository(MyContext context)
        {
            _context = context;
        }
        public int Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                return _context.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                return _context.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string[] includeRelationalTables = null)
        {
            try
            {
                //select * from tabloAdi
                IQueryable<T> query = _context.Set<T>();
                if (filter != null)
                {
                    //eğer koşul verdiyse select*fro tabloAdi where koşullar
                    query = query.Where(filter);
                }
                if (includeRelationalTables != null)
                {
                    //ilişkiliTabloAdi1,ilişkiliTabloAdi2...
                    foreach (var item in includeRelationalTables) 
                    {
                        query = query.Include(item); //join yapıyor
                    }
                }
                return query.AsNoTracking();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public T GetByConditions(Expression<Func<T, bool>>? filter = null, string[] includeRelationalTables = null)
        {
            try
            {
                //select * from tabloAdi
                IQueryable<T> query = _context.Set<T>();
                if (filter != null)
                {
                    //eğer koşul verdiyse select*fro tabloAdi where koşullar
                    query = query.Where(filter);
                }
                if (includeRelationalTables != null)
                {
                    //ilişkiliTabloAdi1,ilişkiliTabloAdi2...
                    foreach (var item in includeRelationalTables)
                    {
                        query = query.Include(item); //join yapıyor
                    }
                }
                return query.AsNoTracking().FirstOrDefault();// query içinden ilk gelen datayı geri gönderir.
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T GetById(Id id)
        {
            try
            {
                return _context.Set<T>().Find(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                return _context.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
