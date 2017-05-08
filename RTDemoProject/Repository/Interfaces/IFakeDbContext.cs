using System.Data.Entity;
using RTDemoProject.Entities;
using RTDemoProject.Entities.Interfaces;
using RTDemoProject.Repository.Fakes;

namespace RTDemoProject.Repository.Interfaces
{
    public interface IFakeDbContext : IDataContextAsync
    {
        DbSet<T> Set<T>() where T : class;

        void AddFakeDbSet<TEntity, TFakeDbSet>()
            where TEntity : class, IObjectState, new()
            where TFakeDbSet : FakeDbSet<TEntity>, IDbSet<TEntity>, new();
    }
}