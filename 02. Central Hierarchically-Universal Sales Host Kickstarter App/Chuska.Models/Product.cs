namespace Chushka.Models
{
    using Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public ProductType Type { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}