using System.Linq;
using RTDemoProject.Entities.POCOs;
using RTDemoProject.Repository.Interfaces;

namespace RTDemoProject.Repository.CustomRepositories
{
    public static class EmployeeRepository
    {
        public static IQueryable<Employee> EmployeeBySite(this IRepository<Employee> repository,
            params string[] departmentNames)
        {
            var departments = repository.GetRepository<Site>().Queryable();

            var resp = repository.Queryable()
                .Join(departments, employee => employee.SiteID, department => department.SiteId,
                    (employee, department) => new {Employee = employee, Department = department})
                .Where(x => departmentNames.Contains(x.Department.SiteName))
                .Select(x => x.Employee);
            return resp;
        }
    }
}