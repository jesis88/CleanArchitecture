﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entity
{
    public class Customer
    {
        public int Id { get; set; }

        [StringLength(80, MinimumLength = 4)]
        public string? Name { get; set; }

        [StringLength(80, MinimumLength = 4)]
        public string? Address { get; set; }

        [StringLength(80, MinimumLength = 4)]
        public string? PhoneNumber { get; set; }

        public bool Active { get; set; } = true;

        [ForeignKey("User")]
        public required string UserId { get; set; }

        [ExcludeFromCodeCoverage]
        // Navigation property
        public User? User { get; set; }
    }
}
