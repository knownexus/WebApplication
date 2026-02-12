using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WidgetService.Models;
using WidgetService.Models.DTOs;

namespace WidgetService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly WidgetService.Services.OrderService _orderService;


        public OrderController(ILogger<OrderController> logger, Services.OrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ResponseModel> CreateOrder([FromBody] NewOrderDto newOrder)
        {
            var result = await _orderService.CreateOrder(newOrder);
            return result;
        }
       
    }
}
