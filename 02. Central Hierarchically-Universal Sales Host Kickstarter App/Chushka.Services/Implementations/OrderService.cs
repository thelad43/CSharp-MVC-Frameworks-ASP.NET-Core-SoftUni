namespace Chushka.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Chushka.Models;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static Common.WebConstants;

    public class OrderService : IOrderService
    {
        private readonly ChushkaDbContext db;

        public OrderService(ChushkaDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<OrderInfoServiceModel>> AllAsync(int page = 1)
            => await this.db
                .Orders
                .OrderByDescending(o => o.OrderedOn)
                .Skip((page - 1) * OrdersPageSize)
                .Take(OrdersPageSize)
                .ProjectTo<OrderInfoServiceModel>()
                .ToListAsync();

        public async Task Create(int productId, string userId)
        {
            var product = await this.db.Products.FindAsync(productId);
            var user = await this.db.Users.FindAsync(userId);

            if (product == null || user == null)
            {
                return;
            }

            var order = new Order
            {
                UserId = user.Id,
                ProductId = product.Id,
                Product = product,
                User = user,
                OrderedOn = DateTime.UtcNow
            };

            await this.db.AddAsync(order);
            await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync()
            => await this.db.Orders.CountAsync();
    }
}