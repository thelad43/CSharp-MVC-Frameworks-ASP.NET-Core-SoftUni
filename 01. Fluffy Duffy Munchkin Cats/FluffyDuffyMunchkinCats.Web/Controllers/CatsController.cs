namespace FluffyDuffyMunchkinCats.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    public class CatsController : Controller
    {
        private readonly ICatService cats;

        public CatsController(ICatService cats)
        {
            this.cats = cats;
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(CatFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.cats.Add(model.Name, model.Age, model.Breed, model.ImageUrl);

            return RedirectToAction(
                nameof(HomeController.Index),
                nameof(HomeController).Replace("Controller", string.Empty));
        }

        public IActionResult Details(int id)
        {
            return View(this.cats.ById(id));
        }
    }
}