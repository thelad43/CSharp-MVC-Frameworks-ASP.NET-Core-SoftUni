namespace Chushka.Web.Areas.Admin.Models
{
    using Chushka.Models.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductFormAdminViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public ProductType Type { get; set; }

        public IEnumerable<ProductType> Types { get; set; }
    }
}