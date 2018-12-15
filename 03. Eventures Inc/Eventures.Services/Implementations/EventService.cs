namespace Eventures.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Eventures.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static Common.WebConstants;

    public class EventService : IEventService
    {
        private readonly EventuresDbContext db;

        public EventService(EventuresDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<EventServiceModel>> AllAsync(int page = 1)
            => await this.db
                .Events
                .Where(e => e.Tickets >= 1)
                .OrderByDescending(e => e.Start)
                .Skip((page - 1) * EventsPageSize)
                .Take(EventsPageSize)
                .ProjectTo<EventServiceModel>()
                .ToListAsync();

        public async Task<Event> ByIdAsync(int eventId)
            => await this.db
                .Events
                .FindAsync(eventId);

        public async Task CreateAsync(string name, string place, DateTime start, DateTime end, int tickets, decimal ticketPrice)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(place))
            {
                return;
            }

            if (ticketPrice <= 0 || tickets <= 0)
            {
                return;
            }

            var newEvent = new Event
            {
                Name = name,
                Place = place,
                Start = start,
                End = end,
                Tickets = tickets,
                TicketPrice = ticketPrice,
            };

            await this.db.AddAsync(newEvent);
            await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync()
            => await this.db
                .Events
                .CountAsync();
    }
}