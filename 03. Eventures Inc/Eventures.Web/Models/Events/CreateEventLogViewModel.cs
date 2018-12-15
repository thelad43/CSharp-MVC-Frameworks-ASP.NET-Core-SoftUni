namespace Eventures.Web.Models.Events
{
    using System;

    public class CreateEventLogViewModel
    {
        public string EventName { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}