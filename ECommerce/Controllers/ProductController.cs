using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _productService.AddProductAsync(product);
            TempData["SuccessMessage"] = "產品已成功新增！";
            return RedirectToAction("Query");
        }

        [HttpGet]
        public async Task<IActionResult> Query(string productName)
        {
            var products = await _productService.GetAllProductsAsync();

            if (!string.IsNullOrEmpty(productName))
            {
                products = products.Where(p => p.ProductName.Contains(productName, StringComparison.OrdinalIgnoreCase));
            }

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            await _productService.UpdateProductAsync(product);
            TempData["SuccessMessage"] = "產品已成功更新！";
            return RedirectToAction("Query");
        }

    }
}
