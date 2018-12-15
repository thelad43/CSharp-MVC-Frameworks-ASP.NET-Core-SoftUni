namespace Eventures.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Common.WebConstants;

    public class User : IdentityUser
    {
        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string UniqueCitizenNumber { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}