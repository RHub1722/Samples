using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTDemoProject.AutoMaper;
using RTDemoProject.Entities.Interfaces;
using RTDemoProject.Entities.POCOs;
using RTDemoProject.Repository;
using RTDemoProject.Repository.Interfaces;
using RTDemoProject.Service;
using RTDemoProject.Test.Fake;

namespace RTDemoProject.Test.Services
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private EmployeeService _employeeService;
        private IDataContextAsync _dataContextAsync;
        private IUnitOfWorkAsync _unitOfWorkAsync;

        private IRepositoryAsync<Employee> _employees;
        private IRepositoryAsync<Site> _departments;
        private IRepositoryAsync<Order> _orders;
        private IRepositoryAsync<SalesOrderDetail> _salesOrderDetails;
        private IRepositoryAsync<Contact> _contacts;
        private IRepositoryAsync<ApplicationUser> _applicationUsers;

        [TestCleanup]
        public void Cleanup()
        {
            _unitOfWorkAsync.Dispose();
            _dataContextAsync.Dispose();
        }

        [TestInitialize]
        public void Init()
        {
            var mapper = CommonMapper.InitializeAutoMapper().CreateMapper();
            _dataContextAsync = new DbFakeContext();
            _unitOfWorkAsync = new UnitOfWork(_dataContextAsync);

            _employees = new Repository<Employee>(_dataContextAsync, _unitOfWorkAsync);
            _departments = new Repository<Site>(_dataContextAsync, _unitOfWorkAsync);
            _orders = new Repository<Order>(_dataContextAsync, _unitOfWorkAsync);
            _salesOrderDetails = new Repository<SalesOrderDetail>(_dataContextAsync, _unitOfWorkAsync);
            _contacts = new Repository<Contact>(_dataContextAsync, _unitOfWorkAsync);
            _applicationUsers = new Repository<ApplicationUser>(_dataContextAsync, _unitOfWorkAsync);

            _employeeService = new EmployeeService(mapper, _employees, _departments, _orders, _salesOrderDetails, _contacts, _applicationUsers, _unitOfWorkAsync);
        }

        [TestMethod]
        public void TestSample()
        {
            var employee = _employees.Find(1);//Sample override for testing
            var employeeByDepartment = _employeeService.GetEmployeeBySite("Head Office");
            Assert.IsNull(employee);
            Assert.IsTrue(employeeByDepartment.Any());

            //sample: Busines Logic
            _employees.Insert(new Employee()
            {
                EmployeeId = 10,
                SiteID = 0,
                JobTitle = "JobTitle",
            });

            employeeByDepartment = _employeeService.GetEmployeeBySite("Head Office");
            Assert.IsTrue(employeeByDepartment.Count == 4);
        }

    }
}
