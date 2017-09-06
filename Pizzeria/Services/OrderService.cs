using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Data;
using Pizzeria.Models;

namespace Pizzeria.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> All()
        {
            return _context.Order.OrderByDescending(o => o.OrderDate).ToList();
        }

        public Order GetOrder(int orderId)
        {
            return _context.Order.FirstOrDefault(x => x.OrderId == orderId);
        }
    }
}
