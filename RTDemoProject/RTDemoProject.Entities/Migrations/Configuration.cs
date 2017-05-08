using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RTDemoProject.Entities.POCOs;
using RTDemoProject.Shared.Enums;

namespace RTDemoProject.Entities.Migrations
{
    public class Configuration : DbMigrationsConfiguration<EFContext>
    {
        private static readonly Random _random = new Random();

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        protected override void Seed(EFContext context)
        {
            if (context.Employees.Any())
                return;

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            for (int i = 1; i < 11; i++)
            {
                var userToInsert = new ApplicationUser {UserName = $"{i}@mail.com", ObjectState = ObjectState.Added};
                userManager.Create(userToInsert, "Password@123");
            }
            context.SaveChanges();

            var customers = new List<Customer>()
            {
                new Customer()
                {
                    FirstName = "Victor",
                    LastName = "Simon",
                    Phone = _random.Next(999999, 9999999).ToString(),
                    CompanyName = "Zaxo",
                    ObjectState = ObjectState.Added
                },
                new Customer()
                {
                    FirstName = "Virgil",
                    LastName = "Shura",
                    Phone = _random.Next(999999, 9999999).ToString(),
                    CompanyName = "Zaxo",
                    ObjectState = ObjectState.Added
                },
                new Customer()
                {
                    FirstName = "Robert",
                    LastName = "Albu",
                    Phone = _random.Next(999999, 9999999).ToString(),
                    CompanyName = "Zaxo",
                    ObjectState = ObjectState.Added
                },
                new Customer()
                {
                    FirstName = "Vadimir",
                    LastName = "Pavlov",
                    Phone = _random.Next(999999, 9999999).ToString(),
                    CompanyName = "Exil",
                    ObjectState = ObjectState.Added
                },
                new Customer()
                {
                    FirstName = "Vitali",
                    LastName = "Cojocaru",
                    Phone = _random.Next(999999, 9999999).ToString(),
                    CompanyName = "Exil",
                    ObjectState = ObjectState.Added
                },
            };
            context.Customers.AddOrUpdate(customers.ToArray());
            context.SaveChanges();


            var sites = new List<Site>();
            sites.Add(new Site()
            {
                SiteName = "Head Office",
                ModifDate = DateTime.Now.AddMonths(5),
                ObjectState = ObjectState.Added
            });
            sites.Add(new Site()
            {
                SiteName = "Branch 1",
                ModifDate = DateTime.Now.AddDays(-50),
                ObjectState = ObjectState.Added
            });
            sites.Add(new Site()
            {
                SiteName = "Branch 2",
                ModifDate = DateTime.Now.AddDays(-20),
                ObjectState = ObjectState.Added
            });
            context.Departments.AddOrUpdate(sites.ToArray());
            context.SaveChanges();

            var list = context.Users.ToList();
            var employees = new List<Employee>();
            for (int i = 0; i < 10; i++)
                if (i < 3)
                    employees.Add(new Employee()
                    {
                        SiteID = sites[0].SiteId,
                        JobTitle = "Saller",
                        ApplicationUser = list[i],
                        ObjectState = ObjectState.Added
                    });
                else if (i >= 3 && i < 6)
                    employees.Add(new Employee()
                    {
                        SiteID = sites[1].SiteId,
                        JobTitle = "Saller",
                        ApplicationUser = list[i],
                        ObjectState = ObjectState.Added
                    });
                else
                    employees.Add(new Employee()
                    {
                        SiteID = sites[2].SiteId,
                        JobTitle = "Manger",
                        ApplicationUser = list[i],
                        ObjectState = ObjectState.Added
                    });
            context.Employees.AddOrUpdate(employees.ToArray());
            context.SaveChanges();

            var orders = new List<Order>();
            for (int i = 0; i < 10; i++)
                orders.Add(new Order()
                {
                    CustomerId = _random.Next(1, 5),
                    EmployeeId = _random.Next(1, 9),
                    OrderDate = DateTime.Now,
                    RequiredDate = DateTime.Now.AddDays(1),
                    ObjectState = ObjectState.Added
                });
            context.Orders.AddOrUpdate(orders.ToArray());
            context.SaveChanges();

            var salesOrderDetails = new List<SalesOrderDetail>();
            for (int i = 0; i < 100; i++)
                salesOrderDetails.Add(new SalesOrderDetail()
                {
                    Discount = 0,
                    ProductId = 0,
                    Qty = _random.Next(1, 10),
                    UnitPrice = _random.Next(20, 800),
                    OrderId = _random.Next(1, 9),
                    ObjectState = ObjectState.Added
                });
            context.SalesOrderDetails.AddOrUpdate(salesOrderDetails.ToArray());
            context.SaveChanges();

            var contacts = new List<Contact>();
            for (int i = 0; i < 10; i++)
            for (int j = 0; j < 2; j++)
                contacts.Add(new Contact()
                {
                    EmployeeID = employees[i].EmployeeId,
                    Adress = RandomString(10),
                    Phone = _random.Next(999999, 9999999).ToString(),
                    ObjectState = ObjectState.Added
                });
            context.Contacts.AddOrUpdate(contacts.ToArray());
            context.SaveChanges();
        }
    }
}