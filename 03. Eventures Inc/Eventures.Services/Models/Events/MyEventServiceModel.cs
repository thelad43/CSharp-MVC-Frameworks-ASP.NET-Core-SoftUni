namespace Eventures.Services.Models.Events
{
    using System;

    public class MyEventServiceModel
    {
        public string EventName { get; set; }

        public int Tickets { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}