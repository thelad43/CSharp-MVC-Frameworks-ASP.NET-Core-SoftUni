namespace Chushka.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime OrderedOn { get; set; }
    }
}