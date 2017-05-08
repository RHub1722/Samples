using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using RTDemoProject.Entities;
using RTDemoProject.Repository.Interfaces;

namespace RTDemoProject.Repository.Fakes
{
    public abstract class FakeDbContext : IFakeDbContext
    {
        private readonly Dictionary<Type, object> _fakeDbSets;

        protected FakeDbContext()
        {
            _fakeDbSets = new Dictionary<Type, object>();
        }

        public int SaveChanges() => default(int);

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
        }


        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => new Task<int>(() => default(int));

        public Task<int> SaveChangesAsync() => new Task<int>(() => default(int));

        public void Dispose()
        {
        }

        public DbSet<T> Set<T>() where T : class => (DbSet<T>) _fakeDbSets[typeof(T)];

        public void AddFakeDbSet<TEntity, TFakeDbSet>() where TEntity : class, IObjectState, new()
            where TFakeDbSet : FakeDbSet<TEntity>, IDbSet<TEntity>, new()
        {
            var fakeDbSet = Activator.CreateInstance<TFakeDbSet>();
            _fakeDbSets.Add(typeof(TEntity), fakeDbSet);
        }

        public void SyncObjectsStatePostCommit()
        {
        }
    }
}