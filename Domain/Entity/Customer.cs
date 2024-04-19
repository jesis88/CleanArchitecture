namespace Domain.Entity
{
    public class Customer
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public bool Active { get; set; } = true;

        // Foreign key for ApplicationUser
        public string? UserId { get; set; }
    }
}
