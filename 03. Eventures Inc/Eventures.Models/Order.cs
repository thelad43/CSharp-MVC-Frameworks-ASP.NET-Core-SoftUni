namespace Eventures.Models
{
    using System;

    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderedOn { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public int Tickets { get; set; }
    }
}