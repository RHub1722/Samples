using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using AutoMapper;
using RTDemoProject.Entities.POCOs;
using RTDemoProject.Repository.CustomRepositories;
using RTDemoProject.Repository.Interfaces;
using RTDemoProject.Shared.DTOs;
using RTDemoProject.Shared.Enums;

namespace RTDemoProject.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryAsync<ApplicationUser> _applicationUsers;
        private readonly IRepositoryAsync<Contact> _contacts;
        private readonly IRepositoryAsync<Employee> _employees;
        private readonly IMapper _mapper;
        private readonly IRepositoryAsync<Order> _orders;
        private readonly IRepositoryAsync<SalesOrderDetail> _salesOrderDetails;
        private readonly IRepositoryAsync<Site> _sites;

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IMapper mapper,
            IRepositoryAsync<Employee> employees,
            IRepositoryAsync<Site> sites,
            IRepositoryAsync<Order> orders,
            IRepositoryAsync<SalesOrderDetail> salesOrderDetails,
            IRepositoryAsync<Contact> contacts,
            IRepositoryAsync<ApplicationUser> applicationUsers,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _employees = employees;
            _sites = sites;
            _orders = orders;
            _salesOrderDetails = salesOrderDetails;
            _contacts = contacts;
            _applicationUsers = applicationUsers;
            _unitOfWork = unitOfWork;
        }

        //Linq sample
        public List<EmployOfTheMonthDto> EmployOfTheMonth(DateTime month)
        {
            var startDate = new DateTime(month.Year, month.Month, 1);
            var endDate = new DateTime(month.Year, month.Month + 1, 1).AddDays(-1);

            var sumEmplBySite = (from empl in _employees.Queryable()
                    join site in _sites.Queryable() on empl.SiteID equals site.SiteId into sit
                    from sitd in sit.DefaultIfEmpty()
                    join order in _orders.Queryable() on empl.EmployeeId equals order.EmployeeId
                    join us in _applicationUsers.Queryable() on empl.ApplicationUser.Id equals us.Id into use
                    from user in use.DefaultIfEmpty()
                    join sod in _salesOrderDetails.Queryable() on order.OrderId equals sod.OrderId

                    where order.OrderDate < endDate && order.OrderDate > startDate 
                    select new
                    {
                        Site = sitd.SiteName,
                        DepartId = (int?) sitd.SiteId,
                        EmpID = empl.EmployeeId,
                        EmplName = user.UserName,
                        Date = order.OrderDate,
                        Price = sod.UnitPrice * sod.Qty - sod.Discount
                    })
                .GroupBy(x => new {x.Site, x.DepartId, x.EmpID, x.EmplName})
                .Select(x => new {x.Key.Site, Name = x.Key.EmplName, Sum = x.Sum(y => y.Price)}).ToList();

            var maxSumInSite = sumEmplBySite.GroupBy(x => x.Site)
                .Select(x => new {Depart = x.Key, Sum = x.Max(y => y.Sum)});

            return (from emplInSit in sumEmplBySite
                    join mSum in maxSumInSite on new {emplInSit.Sum, Sit = emplInSit.Site} equals
                    new {mSum.Sum, Sit = mSum.Depart}
                    select new EmployOfTheMonthDto {Name = emplInSit.Name, TotalSales = mSum.Sum, Site = emplInSit.Site})
                .ToList();
        }

        //unit of work sample
        public void ChangeChief(int employeeId, int chiefId)
        {
            if (employeeId == chiefId)
                throw new Exception("Invalid chief ContactId");

            _unitOfWork.BeginTransaction(); //unit of work sample
            var employee = _employees.Find(employeeId);
            if (employee == null || !_employees.Queryable().Any(x => x.EmployeeId == chiefId))
                throw new ObjectNotFoundException();

            employee.EmployeeChiefID = chiefId;
            employee.ObjectState = ObjectState.Modified;
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
        }

        //Automaper sample
        public List<ContactDto> GetAllEmployeeContacts(int employeeId)
        {
            return _employees.Queryable()
                .Join(_contacts.Queryable(), employee => employee.EmployeeId, contact => contact.EmployeeID,
                    (employee, contact) => new {Employee = employee, Contact = contact})
                .Where(x => x.Employee.EmployeeId == employeeId).Select(x => x.Contact).ToList()
                .Select(x => _mapper.Map<ContactDto>(x)).ToList();
        }

        //Custom repository sample
        public List<EmployeeDto> GetEmployeeBySite(params string[] name)
            => _employees.EmployeeBySite(name).ToList().Select(x => _mapper.Map<EmployeeDto>(x)).ToList();
    }
}