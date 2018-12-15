namespace Eventures.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Eventures.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Models.Events;
    using Services.Models.Orders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static Common.WebConstants;

    public class OrderService : IOrderService
    {
        private readonly EventuresDbContext db;

        public OrderService(EventuresDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AdminEventServiceModel>> AllAsync(int page = 1)
            => await this.db
                .Orders
                .OrderByDescending(o => o.OrderedOn)
                .Skip((page - 1) * EventsPageSize)
                .Take(EventsPageSize)
                .ProjectTo<AdminEventServiceModel>()
                .ToListAsync();

        public async Task CreateAsync(User user, Event currentEvent, int ticketsCount)
        {
            if (user == null || currentEvent == null || ticketsCount <= 0)
            {
                return;
            }

            var order = new Order
            {
                User = user,
                Event = currentEvent,
                Tickets = ticketsCount,
                OrderedOn = DateTime.UtcNow,
                EventId = currentEvent.Id,
                UserId = user.Id
            };

            currentEvent.Tickets -= ticketsCount;

            await this.db.AddAsync(order);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<MyEventServiceModel>> MyAsync(string userId, int page = 1)
            => await this.db
                .Orders
                .Where(o => o.User.Id == userId)
                .OrderByDescending(o => o.OrderedOn)
                .Skip((page - 1) * EventsPageSize)
                .Take(EventsPageSize)
                .ProjectTo<MyEventServiceModel>()
                .ToListAsync();

        public async Task<int> TotalAsyncByUserId(string userId)
            => await this.db
                .Orders
                .Where(o => o.User.Id == userId)
                .CountAsync();

        public async Task<int> TotalAsync()
            => await this.db
                .Orders
                .CountAsync();
    }
}