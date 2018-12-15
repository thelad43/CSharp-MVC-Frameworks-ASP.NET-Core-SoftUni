namespace Eventures.Web.Controllers
{
    using Eventures.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Events;
    using Services;
    using System.Threading.Tasks;

    [Authorize]
    public class EventsController : Controller
    {
        private readonly IEventService events;
        private readonly IOrderService orders;
        private readonly UserManager<User> userManager;

        public EventsController(
            IEventService events,
            IOrderService orders,
            UserManager<User> userManager)
        {
            this.events = events;
            this.orders = orders;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(int pageNumber = 1)
            => View(new AllEventsListingViewModel
            {
                Events = await this.events.AllAsync(pageNumber),
                TotalEvents = await this.events.TotalAsync(),
                CurrentPage = pageNumber,
            });

        [HttpGet]
        public async Task<IActionResult> My(int pageNumber = 1)
        {
            var userId = this.userManager.GetUserId(User);
            var events = await this.orders.MyAsync(userId, pageNumber);

            var model = new MyEventsListingViewModel
            {
                Events = events,
                TotalEvents = await this.orders.TotalAsyncByUserId(userId),
                CurrentPage = pageNumber
            };

            return View(model);
        }
    }
}