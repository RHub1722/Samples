using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RTDemoProject.Entities.POCOs
{
    public class Employee : Entity
    {
        public int EmployeeId { get; set; }
        public string JobTitle { get; set; }
        public virtual Site Site { get; set; }
        public virtual int? SiteID { get; set; }

        public virtual Employee EmployeeChif { get; set; }
        public virtual int? EmployeeChiefID { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class EmployeeMap : EntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            HasKey(x => x.EmployeeId);
            ToTable("Employees");
            Property(x => x.EmployeeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.JobTitle).IsRequired().HasMaxLength(50);

            // Relationships
            HasOptional(x => x.Site).WithMany(x => x.Employees).HasForeignKey(x => x.SiteID);
            HasOptional(x => x.EmployeeChif).WithMany().HasForeignKey(x => x.EmployeeChiefID);
            HasOptional(x => x.ApplicationUser).WithOptionalDependent(x => x.Employee);

            HasMany(x => x.Orders);
        }
    }
}