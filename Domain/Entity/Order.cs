namespace Domain.Entity
{
    public class Order
    {
        public int Id { get; set; } 

        public string? ProductName { get; set; }

        public int? Quantity { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount {  get; set; }

        //Fk
        public int CustomerId { get; set; }
    }
}