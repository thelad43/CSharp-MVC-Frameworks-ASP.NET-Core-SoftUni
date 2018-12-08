namespace Chushka.Web.Controllers
{
    using Chushka.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService products;

        public ProductsController(IProductService products)
        {
            this.products = products;
        }

        public async Task<IActionResult> Details(int id)
            => View(await this.products.ById(id));
    }
}