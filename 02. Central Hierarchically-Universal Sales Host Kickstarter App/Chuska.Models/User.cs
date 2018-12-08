namespace Chushka.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}