using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RTDemoProject.Entities.POCOs
{
    public class Customer : Entity
    {
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            HasKey(x => x.CustomerId);
            ToTable("Customers");
            Property(x => x.CustomerId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CompanyName).IsRequired().HasMaxLength(50);
            Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            Property(x => x.LastName).IsRequired().HasMaxLength(50);
            Property(x => x.Phone).HasMaxLength(50);

            // Relationships
            HasMany(x => x.Orders).WithRequired(x => x.Customer).WillCascadeOnDelete(true);
        }
    }
}