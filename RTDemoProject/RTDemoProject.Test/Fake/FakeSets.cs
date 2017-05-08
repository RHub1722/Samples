using System;
using RTDemoProject.Entities.POCOs;
using RTDemoProject.Repository.Fakes;
using RTDemoProject.Shared.Enums;

namespace RTDemoProject.Test.Fake
{
    public class EmployeeFakeSets : FakeDbSet<Employee>
    {
        //sample: fake seed
        public EmployeeFakeSets()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i < 3)
                    Add(new Employee() { SiteID = 0, JobTitle = $"Saller {i}", ObjectState = ObjectState.Added});
                else if (i >= 3 && i < 7)
                    Add(new Employee() { SiteID = 1, JobTitle = $"Saller {i}", ObjectState = ObjectState.Added});
                else
                    Add(new Employee() { SiteID = 2, JobTitle = $"Manger {i}", ObjectState = ObjectState.Added});
            }
        }
        public override Employee Find(params object[] keyValues) => null;//Simulation
    }

    public class ContactFakeSets : FakeDbSet<Contact>
    {

    }

    public class SiteFakeSets : FakeDbSet<Site>
    {
        public SiteFakeSets()
        {
            Add(new Site() { SiteName = "Head Office", ModifDate = DateTime.Now, ObjectState = ObjectState.Added });
            Add(new Site() { SiteName = "Branch 1", ModifDate = DateTime.Now.AddDays(-5), ObjectState = ObjectState.Added });
            Add(new Site() { SiteName = "Branch 2", ModifDate = DateTime.Now.AddDays(-20), ObjectState = ObjectState.Added });
        }
    }

    public class OrderFakeSets : FakeDbSet<Order>
    {

    }

    public class SalesOrderDetailFakeSets : FakeDbSet<SalesOrderDetail>
    {

    }

    public class ApplicationUserFakeSets : FakeDbSet<ApplicationUser>
    {

    }
}
