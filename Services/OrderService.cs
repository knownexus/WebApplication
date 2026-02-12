using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WidgetService.Controllers;
using WidgetService.Data;
using WidgetService.Models;
using WidgetService.Models.DTOs;

namespace WidgetService.Services
{
    public class OrderService
    {
        private readonly ILogger<OrderService> _logger;

        private readonly AppDbContext _context;
        public OrderService(AppDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseModel> CreateOrder(NewOrderDto newOrder)
        {
            if (newOrder.Items.Count == 0)
            {
                return new ErrorModel
                {
                    Success = false,
                    Error = "Order contained no items",
                    Code = "ORDER_EMPTY"

                };
            }

            decimal totalValue = 0;
            foreach (var item in newOrder.Items)
            {
                var widget = await _context.Widget.Where(w => w.Id == item.WidgetId && w.IsArchived == false).FirstOrDefaultAsync();
                if (widget.QuantityAvailable < item.Quantity)
                {
                    return new BadRequestResponseModel();
                }

                widget.QuantityAvailable -= item.Quantity;
                totalValue += (widget.UnitPrice * item.Quantity);

            }
            await _context.SaveChangesAsync();
            var reference = GenerateOrderReference();
            return new OrderSuccessResponseModel
            {
                Success = true,
                Message = "Order placed successfully",
                OrderReference = reference,
                TotalAmount = totalValue
            };

        }


        private string GenerateOrderReference()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var guidPart = Guid.NewGuid()
                .ToString("N")          // 32 chars, no hyphens
                .Substring(0, 6)        // truncate
                .ToUpperInvariant();

            return $"ORD-{datePart}-{guidPart}";
        }

    }
}
