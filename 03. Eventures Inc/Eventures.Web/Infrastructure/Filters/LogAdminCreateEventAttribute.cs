namespace Eventures.Web.Infrastructure.Filters
{
    using Areas.Admin.Models.Events;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;

    public class LogAdminCreateEventAttribute : ActionFilterAttribute
    {
        private readonly ILogger logger;
        private CreateEventFormModel model;

        public LogAdminCreateEventAttribute(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<LogAdminCreateEventAttribute>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.model = context
                .ActionArguments
                .Values
                .OfType<CreateEventFormModel>()
                .Single();

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (this.model != null)
            {
                var username = context.HttpContext.User.Identity.Name;
                var dateTime = DateTime.UtcNow.ToUniversalTime();
                var eventName = this.model.Name;
                var eventStart = this.model.Start.ToUniversalTime();
                var eventEnd = this.model.End.ToUniversalTime();

                this.logger.LogInformation($"[{dateTime}] Administrator {username} create event {eventName} ({eventStart} / {eventEnd}).");
            }

            base.OnActionExecuted(context);
        }
    }
}