using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using RTDemoProject.Entities.Interfaces;
using RTDemoProject.Entities.POCOs;
using RTDemoProject.Shared.Helpers;

namespace RTDemoProject.Entities
{
    public class EFContext : IdentityDbContext<ApplicationUser>, IDataContextAsync
    {
        private readonly Guid _instanceId;

        bool _disposed;

        public EFContext() : base("EFContext", throwIfV1Schema: false)
        {
            //for modif model
            //Database.SetInitializer<EFContext>(new DropCreateDatabaseIfModelChanges<EFContext>());
            _instanceId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public Guid InstanceId => _instanceId;

        public DbSet<Site> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new SalesOrderDetailMap());

            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new ApplicationUserMap());
            modelBuilder.Configurations.Add(new ContactMap());
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //disposing instances
                }

                _disposed = true;
            }
            base.Dispose(disposing);
        }


        public static EFContext Create() => new EFContext();

        #region EFActions

        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }


        public override async Task<int> SaveChangesAsync() => await SaveChangesAsync(CancellationToken.None);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState) dbEntityEntry.Entity).ObjectState);
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                ((IObjectState) dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
        }

        #endregion
    }
}