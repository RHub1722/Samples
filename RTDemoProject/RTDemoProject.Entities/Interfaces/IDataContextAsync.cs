using System.Threading;
using System.Threading.Tasks;

namespace RTDemoProject.Entities.Interfaces
{
    public interface IDataContextAsync : IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
    }
}