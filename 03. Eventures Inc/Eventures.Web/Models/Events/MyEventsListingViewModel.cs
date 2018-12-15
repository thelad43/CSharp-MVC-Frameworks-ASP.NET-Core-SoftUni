namespace Eventures.Web.Models.Events
{
    using Eventures.Common;
    using Eventures.Services.Models.Events;
    using System;
    using System.Collections.Generic;

    public class MyEventsListingViewModel
    {
        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int TotalPages => (int)Math.Ceiling((double)this.TotalEvents / WebConstants.EventsPageSize);

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;

        public int TotalEvents { get; set; }

        public IEnumerable<MyEventServiceModel> Events { get; set; }
    }
}