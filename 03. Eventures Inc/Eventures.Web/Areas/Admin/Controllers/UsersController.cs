namespace Eventures.Web.Areas.Admin.Controllers
{
    using Eventures.Common;
    using Eventures.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Models.Users;
    using Services.Admin;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : BaseAdminController
    {
        private readonly IAdminUserService users;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(
            IAdminUserService users,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.users = users;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await this.users.AllAsync();

            var roles = await this.roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToListAsync();

            var model = new UserListingsViewModel
            {
                Users = users,
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(AddUserToRoleFormModel model)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);

            var userExists = user != null;

            if (!roleExists || !userExists)
            {
                ModelState.AddModelError(string.Empty, "Invalid identity details!");
            }

            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage("Invalid identity details! No roles updated!");
                return RedirectToAction(nameof(UsersController.Index));
            }

            if (model.Role == WebConstants.UserRole)
            {
                await this.userManager.RemoveFromRoleAsync(user, WebConstants.AdministratorRole);
            }
            else
            {
                await this.userManager.AddToRoleAsync(user, model.Role);
            }

            TempData.AddSuccessMessage($"User {user.UserName} successfully added to the {model.Role} role.");

            return RedirectToAction(nameof(UsersController.Index));
        }
    }
}