namespace Chushka.Web.Models.Products
{
    using Chushka.Common;
    using Chushka.Services.Models;
    using System;
    using System.Collections.Generic;

    public class ProductsListingViewModel
    {
        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int TotalPages => (int)Math.Ceiling((double)this.TotalProducts / WebConstants.ProductsPageSize);

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;

        public int TotalProducts { get; set; }

        public IEnumerable<ProductInfoServiceModel> Products { get; set; }
    }
}