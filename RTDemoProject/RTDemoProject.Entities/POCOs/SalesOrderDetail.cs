using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RTDemoProject.Entities.POCOs
{
    public class SalesOrderDetail : Entity
    {
        public int SalesOrderDetailId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Error")]
        public decimal Qty { get; set; }

        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Error")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Error")]
        public decimal Discount { get; set; }

        public Order Order { get; set; }
        public int OrderId { get; set; }
    }

    public class SalesOrderDetailMap : EntityTypeConfiguration<SalesOrderDetail>
    {
        public SalesOrderDetailMap()
        {
            HasKey(x => x.SalesOrderDetailId);
            ToTable("SalesOrderDetails");
            Property(x => x.SalesOrderDetailId).HasColumnName("SalesOrderDetailID");

            Property(x => x.SalesOrderDetailId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UnitPrice).HasColumnType("Money");
            Property(x => x.Discount).HasColumnType("Money");

            HasRequired(x => x.Order).WithMany(x => x.SalesOrderDetails).HasForeignKey(x => x.OrderId);
        }
    }
}