using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using SW10.SWMANAGER.EntityFramework;
using SW10.SWMANAGER.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Repositorios
{
    public class SWRepository<T> : IDisposable, IRepository<T, long> where T : CamposPadraoCRUD
    {
        private IAbpSession _abpSession;
        private SWMANAGERDbContext swContext;
        // public SWRepository() { }

        //public SWRepository(IAbpSession abpSession, DbContext context=null)
        // {
        //     _abpSession = abpSession;

        //     if (context != null)
        //     {
        //         swContext = (SWMANAGERDbContext)context;// new SWMANAGERDbContext("American");
        //     }
        //     else
        //     {
        //         swContext = new SWMANAGERDbContext("American");
        //     }
        //     swContext.AbpSession = _abpSession;

        // }


        public SWRepository(IAbpSession abpSession, ISessionAppService _sessionService)
        {


            var user = ((SW10.SWMANAGER.SWMANAGERAppServiceBase)_sessionService).UserManager;
            var aewrer = user.Users;


            var contexts = ((Abp.EntityFramework.Uow.EfUnitOfWork)(((Abp.AbpServiceBase)_sessionService).UnitOfWorkManager.Current)).GetAllActiveDbContexts();

            DbContext context = null;

            if (contexts.Count > 0)
            {
                context = contexts[0];
            }




            _abpSession = abpSession;

            if (context != null)
            {
                swContext = (SWMANAGERDbContext)context;// new SWMANAGERDbContext("American");
            }
            else
            {
                swContext = new SWMANAGERDbContext("American");
            }
            swContext.AbpSession = _abpSession;

        }


        //****************

        //NÃO ESQUECER DO ISDELETE

        //**************

        public int Count()
        {
            return swContext.Set<T>().Count();
            //var teste = swContext.Set<T>();

            // teste.Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return swContext.Set<T>().Count(predicate);
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            // entity.CreatorUserId = _abpSession.UserId;
            swContext.Set<T>().Remove(entity);
            swContext.SaveChanges();
        }

        public Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(long id)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(long id)
        {
            throw new NotImplementedException();
        }

        public T Get(long id)
        {
            return GetAll().Where(w => w.Id == id).SingleOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return swContext.Set<T>().AsQueryable();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = swContext.Set<T>().AsQueryable();

            query = propertySelectors.Aggregate(query,
                        (current, include) => current.Include(include));

            return query;
        }

        public List<T> GetAllList()
        {
            return swContext.Set<T>().AsQueryable().ToList();
        }

        public List<T> GetAllList(Expression<Func<T, bool>> predicate)
        {
            return swContext.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public Task<List<T>> GetAllListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public T Insert(T entity)
        {
            entity.CreatorUserId = _abpSession.UserId;
            var result = swContext.Set<T>().Add(entity);
            swContext.SaveChanges();
            return result;
        }

        public long InsertAndGetId(T entity)
        {
            entity.CreatorUserId = _abpSession.UserId;
            var result = swContext.Set<T>().Add(entity);
            swContext.SaveChanges();
            return result.Id;
        }

        public async Task<long> InsertAndGetIdAsync(T entity)
        {
            entity.CreatorUserId = _abpSession.UserId;
            var result = swContext.Set<T>().Add(entity);
            swContext.SaveChanges();
            return result.Id;
        }

        public Task<T> InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public T InsertOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }

        public long InsertOrUpdateAndGetId(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<long> InsertOrUpdateAndGetIdAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> InsertOrUpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public T Load(long id)
        {
            throw new NotImplementedException();
        }

        public long LongCount()
        {
            throw new NotImplementedException();
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T1 Query<T1>(Func<IQueryable<T>, T1> queryMethod)
        {
            throw new NotImplementedException();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            var result = swContext.Set<T>().Where(w => w.Id == entity.Id).FirstOrDefault();
            entity.CreatorUserId = _abpSession.UserId;
            entity.LastModifierUserId = _abpSession.UserId;

            swContext.Entry(result).CurrentValues.SetValues(entity);


            //entity.CreatorUserId = _abpSession.UserId;
            //entity.LastModifierUserId = _abpSession.UserId;

            //result.CreatorUserId = _abpSession.UserId;
            //result.LastModifierUserId = _abpSession.UserId;



            // swContext.AbpSession = _abpSession;



            //swContext.Entry(entity).State = EntityState.Modified;

            //var result = swContext.Set<T>().Where(w => w.Id == entity.Id).FirstOrDefault();



            //swContext.Entry(entity).State = EntityState.Modified;
            //entity.LastModifierUserId = _abpSession.UserId;

            //var propriedades = result.GetType().GetProperties();

            //foreach (var item in propriedades)
            //{
            //    result.GetType().GetProperty(item.Name).SetValue(result, entity.GetType().GetProperty(item.Name).GetValue(entity));
            //}

            swContext.SaveChanges();
            // entity.LastModifierUserId = _abpSession.UserId;
            return entity;
        }

        public T Update(long id, Action<T> updateAction)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(long id, Func<T, Task> updateAction)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
