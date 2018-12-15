namespace Eventures.Services
{
    using Eventures.Models;
    using Eventures.Services.Models.Events;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventService
    {
        Task<IEnumerable<EventServiceModel>> AllAsync(int page = 1);

        Task CreateAsync(string name, string place, DateTime start, DateTime end, int tickets, decimal ticketPrice);

        Task<int> TotalAsync();

        Task<Event> ByIdAsync(int eventId);
    }
}