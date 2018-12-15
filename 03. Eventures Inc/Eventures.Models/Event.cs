namespace Eventures.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Common.WebConstants;

    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Place { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Tickets { get; set; }

        public decimal TicketPrice { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}