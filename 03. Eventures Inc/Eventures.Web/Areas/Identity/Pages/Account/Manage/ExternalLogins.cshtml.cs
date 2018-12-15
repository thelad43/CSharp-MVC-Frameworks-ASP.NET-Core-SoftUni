namespace Eventures.Web.Areas.Identity.Pages.Account.Manage
{
    using Eventures.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public ExternalLoginsModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationScheme> OtherLogins { get; set; }

        public bool ShowRemoveButton { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            await NewMethod(user);

            OtherLogins = (await this.signInManager
                .GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            ShowRemoveButton = user.PasswordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        private async Task NewMethod(User user)
        {
            CurrentLogins = await userManager.GetLoginsAsync(user);
        }

        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var result = await this.userManager.RemoveLoginAsync(user, loginProvider, providerKey);

            if (!result.Succeeded)
            {
                var userId = await this.userManager.GetUserIdAsync(user);

                throw new InvalidOperationException($"Unexpected error occurred removing external login for user with ID '{userId}'.");
            }

            await this.signInManager.RefreshSignInAsync(user);

            StatusMessage = "The external login was removed.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");

            var properties = this.signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl, this.userManager.GetUserId(User));

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync(await this.userManager.GetUserIdAsync(user));

            if (info == null)
            {
                throw new InvalidOperationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await this.userManager.AddLoginAsync(user, info);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";

            return RedirectToPage();
        }
    }
}