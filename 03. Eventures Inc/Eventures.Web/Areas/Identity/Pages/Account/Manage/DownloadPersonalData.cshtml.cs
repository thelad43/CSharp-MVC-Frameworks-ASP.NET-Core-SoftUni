namespace Eventures.Web.Areas.Identity.Pages.Account.Manage
{
    using Eventures.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<DownloadPersonalDataModel> logger;

        public DownloadPersonalDataModel(
            UserManager<User> userManager,
            ILogger<DownloadPersonalDataModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            this.logger.LogInformation("User with ID '{UserId}' asked for their personal data.", this.userManager.GetUserId(User));

            var personalData = new Dictionary<string, string>();

            var personalDataProps = typeof(User)
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            this.Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");

            return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)), "text/json");
        }
    }
}