using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductAPIController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductAPIController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("GetProduct/{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
    }

    [HttpPut("UpdateProduct")]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        await _productService.UpdateProductAsync(product);
        return NoContent();
    }

    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}
