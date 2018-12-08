namespace Chushka.Services.Models
{
    using Chushka.Models.Enums;

    public class ProductDetailsServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public ProductType Type { get; set; }
    }
}