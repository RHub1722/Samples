using System.Data.Entity.ModelConfiguration;

namespace RTDemoProject.Entities.POCOs
{
    public class Contact : Entity
    {
        public int ContactId { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual int EmployeeID { get; set; }
    }

    public class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            HasKey(x => x.ContactId);

            ToTable("Contacts");

            Property(x => x.Phone).IsRequired();

            HasRequired(x => x.Employee)
                .WithMany(x => x.Contacts)
                .HasForeignKey(x => x.EmployeeID)
                .WillCascadeOnDelete(true);
        }
    }
}