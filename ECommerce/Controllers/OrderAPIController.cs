using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderAPIController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderAPIController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("AddOrderWithDetails")]
    public async Task<IActionResult> AddOrderWithDetails([FromBody] OrderViewModel orderViewModel)
    {
        try
        {
            await _orderService.AddOrderWithDetailsAsync(orderViewModel);
            return CreatedAtAction(nameof(GetOrderWithDetails), new { id = orderViewModel.OrderId }, orderViewModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetOrderWithDetails/{id}")]
    public async Task<IActionResult> GetOrderWithDetails(int id)
    {
        var orderViewModel = await _orderService.GetOrderWithDetailsAsync(id);
        if (orderViewModel == null)
        {
            return NotFound();
        }
        return Ok(orderViewModel);
    }

    [HttpPut("UpdateOrderWithDetails")]
    public async Task<IActionResult> UpdateOrderWithDetails([FromBody] OrderViewModel orderViewModel)
    {
        try
        {
            await _orderService.UpdateOrderWithDetailsAsync(orderViewModel);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
