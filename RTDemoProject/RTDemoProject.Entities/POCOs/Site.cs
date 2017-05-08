using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RTDemoProject.Entities.POCOs
{
    public class Site : Entity
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public DateTime ModifDate { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }

    public class DepartmentMap : EntityTypeConfiguration<Site>
    {
        public DepartmentMap()
        {
            HasKey(x => x.SiteId);
            Property(x => x.SiteId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("Sites");
            Property(x => x.SiteName).HasMaxLength(70);
            HasMany(x => x.Employees).WithOptional(x => x.Site);
        }
    }
}