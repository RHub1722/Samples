namespace RTDemoProject.Shared.DTOs
{
    public class SalesOrderDetailDto
    {
        public int SalesOrderDetailId { get; set; }
        public decimal Qty { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discuont { get; set; }
    }
}
