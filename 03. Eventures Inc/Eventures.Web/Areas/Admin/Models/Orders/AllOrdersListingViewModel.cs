namespace Eventures.Web.Areas.Admin.Models.Orders
{
    using Eventures.Common;
    using Services.Models.Orders;
    using System;
    using System.Collections.Generic;

    public class AllOrdersListingViewModel
    {
        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int TotalPages => (int)Math.Ceiling((double)this.TotalOrders / WebConstants.EventsPageSize);

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;

        public int TotalOrders { get; set; }

        public IEnumerable<AdminEventServiceModel> Orders { get; set; }
    }
}