namespace Eventures.Services.Models.Events
{
    using System;

    public class EventServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Place { get; set; }

        public int Tickets { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}