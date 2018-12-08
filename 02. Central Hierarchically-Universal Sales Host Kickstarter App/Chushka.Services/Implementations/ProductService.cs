namespace Chushka.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Chushka.Models;
    using Chushka.Models.Enums;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static Common.WebConstants;

    public class ProductService : IProductService
    {
        private readonly ChushkaDbContext db;

        public ProductService(ChushkaDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<ProductInfoServiceModel>> AllAsync(int page = 1)
            => await this.db
                .Products
                .OrderBy(p => p.Price)
                .Skip((page - 1) * ProductsPageSize)
                .Take(ProductsPageSize)
                .ProjectTo<ProductInfoServiceModel>()
                .ToListAsync();

        public async Task<ProductDetailsServiceModel> ById(int id)
            => await this.db
                .Products
                .Where(p => p.Id == id)
                .ProjectTo<ProductDetailsServiceModel>()
                .FirstOrDefaultAsync();

        public async Task CreateAsync(string name, decimal price, string description, ProductType type)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
            {
                return;
            }

            if (price < 0)
            {
                return;
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                Description = description,
                Type = type
            };

            await this.db.AddAsync(product);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var product = await this.db.Products.FindAsync(id);

            if (product == null)
            {
                return;
            }

            this.db.Products.Remove(product);
            await this.db.SaveChangesAsync();
        }

        public async Task Edit(int id, string name, decimal price, string description, ProductType type)
        {
            var product = await this.db.Products.FindAsync(id);

            if (product == null)
            {
                return;
            }

            product.Name = name;
            product.Price = price;
            product.Description = description;
            product.Type = type;

            await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync()
            => await this.db.Products.CountAsync();
    }
}