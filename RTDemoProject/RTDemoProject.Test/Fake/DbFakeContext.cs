using RTDemoProject.Entities.POCOs;
using RTDemoProject.Repository.Fakes;

namespace RTDemoProject.Test.Fake
{
    public class DbFakeContext : FakeDbContext
    {
        public DbFakeContext()
        {
            AddFakeDbSet<Employee, EmployeeFakeSets>();
            AddFakeDbSet<Contact, ContactFakeSets>();
            AddFakeDbSet<Site, SiteFakeSets>();
            AddFakeDbSet<Order, OrderFakeSets>();
            AddFakeDbSet<SalesOrderDetail, SalesOrderDetailFakeSets>();
            AddFakeDbSet<ApplicationUser, ApplicationUserFakeSets>();
        }
    }
}
