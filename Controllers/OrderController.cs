// Controllers/OrderController.cs
using ECommerce.Contracts;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpPost("GetMostRecentOrder")]
    public async Task<IActionResult> GetMostRecentOrder(Request request)
    {
        try
        {
            var order = await _orderRepository.GetMostRecentOrderAsync(request);

            if (order == null)
            {
                return BadRequest("Invalid email or customer ID.");
            }

            return Ok(order);
        }
        catch (Exception ex)
        {

            throw ex;
        }
  

    }
}
