using Application.Interfaces;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class CustomerWrapper(CqrsDbContext context) : ICustomerWrapper
    {
        private readonly CqrsDbContext _context = context;

        public async Task<int> AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            return await _context.SaveChangesAsync();
        }
    }
}
