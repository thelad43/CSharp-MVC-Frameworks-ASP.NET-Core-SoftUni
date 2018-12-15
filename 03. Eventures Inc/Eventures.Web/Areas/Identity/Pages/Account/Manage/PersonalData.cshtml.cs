namespace Eventures.Web.Areas.Identity.Pages.Account.Manage
{
    using Eventures.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<PersonalDataModel> logger;

        public PersonalDataModel(
            UserManager<User> userManager,
            ILogger<PersonalDataModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}