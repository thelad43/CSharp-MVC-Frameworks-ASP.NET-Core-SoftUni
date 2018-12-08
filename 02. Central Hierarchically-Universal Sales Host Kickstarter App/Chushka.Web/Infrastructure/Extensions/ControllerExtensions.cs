namespace Chushka.Web.Infrastructure.Extensions
{
    using Controllers;
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerExtensions
    {
        public static IActionResult RedirectToHomeIndex(this Controller controller)
        {
            return controller.RedirectToAction(
                nameof(HomeController.Index).Replace("Controller", string.Empty),
                nameof(HomeController).Replace("Controller", string.Empty),
                new { area = string.Empty });
        }
    }
}