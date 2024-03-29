using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entity
{
    public class Order
    {
        public int OrderId { get; set; } 

        [StringLength(80, MinimumLength = 4)]
        public string? ProductName { get; set; }

        public int? Quantity { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [ExcludeFromCodeCoverage]
        // Navigation property
        public Customer? Customer { get; set; }
    }
}