namespace Chushka.Web.Models.Orders
{
    using Chushka.Common;
    using Chushka.Services.Models;
    using System;
    using System.Collections.Generic;

    public class OrdersListingViewModel
    {
        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int TotalPages => (int)Math.Ceiling((double)this.TotalOrders / WebConstants.OrdersPageSize);

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;

        public int TotalOrders { get; set; }

        public IEnumerable<OrderInfoServiceModel> Orders { get; set; }
    }
}