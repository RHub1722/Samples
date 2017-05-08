using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RTDemoProject.Entities;
using RTDemoProject.Entities.Interfaces;
using RTDemoProject.Repository.Fakes;
using RTDemoProject.Repository.Interfaces;
using RTDemoProject.Shared.Enums;

namespace RTDemoProject.Repository
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        private readonly IDataContextAsync _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public Repository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

            //set poco type
            var dbContext = context as DbContext;

            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
            else
            {
                var fakeContext = context as FakeDbContext;

                if (fakeContext != null)
                    _dbSet = fakeContext.Set<TEntity>();
            }
        }

        public virtual TEntity Find(params object[] keyValues) => _dbSet.Find(keyValues);

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
            => _dbSet.SqlQuery(query, parameters).AsQueryable();

        public virtual void Insert(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Insert(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

        public virtual void Update(TEntity entity)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual IQueryable<TEntity> Queryable()
        {
            var memberInfo = _dbSet?.GetType().BaseType;
            if (memberInfo != null && memberInfo.Name.Contains("FakeDbSet"))
                return _dbSet.ToList().AsQueryable();
            return _dbSet;
        }

        public IRepository<T> GetRepository<T>() where T : class, IObjectState => _unitOfWork.Repository<T>();

        public virtual async Task<TEntity> FindAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
            => await _dbSet.FindAsync(cancellationToken, keyValues);

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
            => await DeleteAsync(CancellationToken.None, keyValues);

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);

            if (entity == null)
                return false;

            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);

            return true;
        }
    }
}