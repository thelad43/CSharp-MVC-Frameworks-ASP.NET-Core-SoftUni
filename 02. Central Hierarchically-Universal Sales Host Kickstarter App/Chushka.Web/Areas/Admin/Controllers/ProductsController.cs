namespace Chushka.Web.Areas.Admin.Controllers
{
    using Chushka.Models.Enums;
    using Chushka.Services;
    using Chushka.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsController : BaseAdminController
    {
        private readonly IProductService products;

        public ProductsController(IProductService products)
        {
            this.products = products;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var types = this.GetTypesOfProduct();

            var model = new ProductFormAdminViewModel
            {
                Types = types
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductFormAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.products.CreateAsync(model.Name, model.Price, model.Description, model.Type);
            return ControllerExtensions.RedirectToHomeIndex(this);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await this.products.ById(id);

            var model = new ProductFormAdminViewModel
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Type = product.Type,
                Types = this.GetTypesOfProduct()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormAdminViewModel model)
        {
            await this.products.Edit(model.Id, model.Name, model.Price, model.Description, model.Type);

            return RedirectToAction(
                nameof(Web.Controllers.ProductsController.Details).Replace("Controller", string.Empty),
                nameof(ProductsController).Replace("Controller", string.Empty),
                new { area = string.Empty, id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await this.products.ById(id);

            var model = new ProductFormAdminViewModel
            {
                Id = id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Type = product.Type,
                Types = this.GetTypesOfProduct()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Destroy(int id)
        {
            await this.products.Delete(id);
            return ControllerExtensions.RedirectToHomeIndex(this);
        }

        private IEnumerable<ProductType> GetTypesOfProduct() =>
            Enum.GetValues(typeof(ProductType)).Cast<ProductType>();
    }
}