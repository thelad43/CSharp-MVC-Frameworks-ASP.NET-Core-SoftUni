namespace Eventures.Services
{
    using Eventures.Models;
    using Eventures.Services.Models.Events;
    using Eventures.Services.Models.Orders;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task<IEnumerable<AdminEventServiceModel>> AllAsync(int page = 1);

        Task<IEnumerable<MyEventServiceModel>> MyAsync(string userId, int page = 1);

        Task CreateAsync(User user, Event currentEvent, int ticketsCount);

        Task<int> TotalAsync();

        Task<int> TotalAsyncByUserId(string userId);
    }
}