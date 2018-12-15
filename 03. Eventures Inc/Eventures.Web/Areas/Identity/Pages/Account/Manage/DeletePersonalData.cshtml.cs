namespace Eventures.Web.Areas.Identity.Pages.Account.Manage
{
    using Eventures.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;

        public DeletePersonalDataModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<DeletePersonalDataModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);

            if (this.RequirePassword)
            {
                if (!await this.userManager.CheckPasswordAsync(user, this.Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password not correct.");
                    return Page();
                }
            }

            var result = await this.userManager.DeleteAsync(user);
            var userId = await this.userManager.GetUserIdAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{userId}'.");
            }

            await this.signInManager.SignOutAsync();

            this.logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}