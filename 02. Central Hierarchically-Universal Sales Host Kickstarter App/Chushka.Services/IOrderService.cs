namespace Chushka.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task Create(int productId, string userId);

        Task<IEnumerable<OrderInfoServiceModel>> AllAsync(int page = 1);

        Task<int> TotalAsync();
    }
}