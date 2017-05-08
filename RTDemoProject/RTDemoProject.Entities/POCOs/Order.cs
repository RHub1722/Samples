using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RTDemoProject.Entities.POCOs
{
    public class Order : Entity
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public Employee Employee { get; set; }
        public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }

    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            HasKey(x => x.OrderId);
            ToTable("Orders");
            Property(x => x.OrderId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.OrderDate).HasColumnType("smalldatetime");
            Property(x => x.RequiredDate).HasColumnType("date");

            HasRequired(x => x.Customer).WithMany(x => x.Orders).HasForeignKey(x => x.CustomerId);
            HasRequired(x => x.Employee).WithMany(x => x.Orders).HasForeignKey(x => x.EmployeeId);
        }
    }
}