namespace Eventures.Web.Controllers
{
    using Eventures.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;

    public class OrdersController : Controller
    {
        private readonly IOrderService orders;
        private readonly IEventService events;
        private readonly UserManager<User> userManager;

        public OrdersController(
            IOrderService orders,
            IEventService events,
            UserManager<User> userManager)
        {
            this.orders = orders;
            this.events = events;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, int tickets)
        {
            if (tickets <= 0)
            {
                TempData.AddErrorMessage("Tickest count cannot be less than zero!");

                return this.RedirectToAction(
                    nameof(EventsController.All),
                    nameof(EventsController).Replace("Controller", string.Empty));
            }

            var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);
            var currentEvent = await this.events.ByIdAsync(id);

            if (currentEvent.Tickets < tickets)
            {
                TempData.AddErrorMessage("There is not enough tickets available!");

                return this.RedirectToAction(
                    nameof(EventsController.All),
                    nameof(EventsController).Replace("Controller", string.Empty));
            }

            await this.orders.CreateAsync(user, currentEvent, tickets);

            var tickestsWord = tickets == 1 ? "ticket" : "tickets";

            TempData.AddSuccessMessage($"Successfully ordered {tickets} {tickestsWord} for {currentEvent.Name}!");

            return RedirectToAction(
                nameof(EventsController.My),
                nameof(EventsController).Replace("Controller", string.Empty));
        }
    }
}