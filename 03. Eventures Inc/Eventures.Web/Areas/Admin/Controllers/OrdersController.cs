namespace Eventures.Web.Areas.Admin.Controllers
{
    using Eventures.Services;
    using Microsoft.AspNetCore.Mvc;
    using Models.Orders;
    using System.Threading.Tasks;

    public class OrdersController : BaseAdminController
    {
        private readonly IOrderService orders;

        public OrdersController(IOrderService orders)
        {
            this.orders = orders;
        }

        [HttpGet]
        public async Task<IActionResult> All(int pageNumber = 1)
             => View(new AllOrdersListingViewModel
             {
                 Orders = await this.orders.AllAsync(pageNumber),
                 TotalOrders = await this.orders.TotalAsync(),
                 CurrentPage = pageNumber,
             });
    }
}