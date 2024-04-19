using Application.Interfaces;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class OrderWrapper(CqrsDbContext context) : IOrderWrapper
    {
        private readonly CqrsDbContext _context = context;

        public async Task<int> AddOrder(Order order)
        {
            _context.Orders.Add(order);
            return await _context.SaveChangesAsync();
        }
    }
}
