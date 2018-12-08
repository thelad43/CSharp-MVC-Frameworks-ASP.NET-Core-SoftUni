namespace Chushka.Web.Controllers
{
    using Chushka.Services;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Products;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IProductService products;

        public HomeController(IProductService products)
        {
            this.products = products;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
            => View(new ProductsListingViewModel
            {
                Products = await this.products.AllAsync(pageNumber),
                TotalProducts = await this.products.TotalAsync(),
                CurrentPage = pageNumber
            });

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}