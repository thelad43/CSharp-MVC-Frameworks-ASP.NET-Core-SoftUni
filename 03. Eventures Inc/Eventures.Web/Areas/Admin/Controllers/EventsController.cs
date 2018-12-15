namespace Eventures.Web.Areas.Admin.Controllers
{
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Events;
    using Services;
    using System.Threading.Tasks;

    public class EventsController : BaseAdminController
    {
        private readonly IEventService events;
        private readonly ILogger<EventsController> logger;

        public EventsController(IEventService events, ILogger<EventsController> logger)
        {
            this.events = events;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ServiceFilter(typeof(LogAdminCreateEventAttribute))]
        public async Task<IActionResult> Create(CreateEventFormModel model)
        {
            if (model.Start >= model.End)
            {
                ModelState.AddModelError(string.Empty, "Start date cannot be after end date!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.events.CreateAsync(
                model.Name,
                model.Place,
                model.Start,
                model.End,
                model.Tickets,
                model.TicketPrice);

            TempData.AddSuccessMessage($"Event created successfully!");

            this.logger.LogInformation($"Event created: {model.Name}", model);

            return RedirectToAction(
                nameof(Web.Controllers.EventsController.All),
                nameof(EventsController).Replace("Controller", string.Empty),
                new { area = string.Empty });
        }
    }
}