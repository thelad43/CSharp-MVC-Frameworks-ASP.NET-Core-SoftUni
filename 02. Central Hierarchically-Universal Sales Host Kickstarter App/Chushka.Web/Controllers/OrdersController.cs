namespace Chushka.Web.Controllers
{
    using Chushka.Services;
    using Chushka.Web.Infrastructure.Extensions;
    using Chushka.Web.Models.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using static Common.WebConstants;

    public class OrdersController : Controller
    {
        private readonly IOrderService orders;

        public OrdersController(IOrderService orders)
        {
            this.orders = orders;
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRole)]
        public async Task<IActionResult> All(int pageNumber = 1)
            => View(new OrdersListingViewModel
            {
                Orders = await this.orders.AllAsync(pageNumber),
                TotalOrders = await this.orders.TotalAsync(),
                CurrentPage = pageNumber
            });

        [HttpGet]
        public async Task<IActionResult> Order(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.orders.Create(id, userId);

            TempData.AddSuccessMessage($"Product ordered successfully!");

            return ControllerExtensions.RedirectToHomeIndex(this);
        }
    }
}