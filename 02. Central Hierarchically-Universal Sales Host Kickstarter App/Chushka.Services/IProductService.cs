namespace Chushka.Services
{
    using Chushka.Models.Enums;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<IEnumerable<ProductInfoServiceModel>> AllAsync(int page = 1);

        Task<int> TotalAsync();

        Task CreateAsync(string name, decimal price, string description, ProductType type);

        Task<ProductDetailsServiceModel> ById(int id);

        Task Edit(int id, string name, decimal price, string description, ProductType type);

        Task Delete(int id);
    }
}