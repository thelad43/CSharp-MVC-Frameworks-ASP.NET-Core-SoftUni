namespace FluffyDuffyMunchkinCats.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private readonly ICatService cats;

        public HomeController(ICatService cats)
        {
            this.cats = cats;
        }

        public IActionResult Index()
            => View(this.cats.All());

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}