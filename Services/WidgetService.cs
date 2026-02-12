using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WidgetService.Controllers;
using WidgetService.Data;
using WidgetService.Models;

namespace WidgetService.Services
{
    public class WidgetService
    {
        private readonly ILogger<WidgetService> _logger;

        private readonly AppDbContext _context;
        public WidgetService(AppDbContext context, ILogger<WidgetService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GetAllWidgetsResponseModel> GetAllWidgetsAsync()
        {
            var widgets =  await _context.Widget.ToListAsync();
            var result = new GetAllWidgetsResponseModel
            {
                Data = widgets,
                Count = widgets.Count
            };

            return result;

        }
        public async Task<GetAllWidgetsResponseModel> GetAllNonArchivedWidgets()
        {
            var widgets = await _context.Widget.Where(w => w.IsArchived == false ).ToListAsync();
            var result = new GetAllWidgetsResponseModel
            {
                Data = widgets,
                Count = widgets.Count
            };

            return result;
        }

        public async Task<WidgetModel?> GetWidgetById(int id)
        {
            var widget = await _context.Widget.Where(w => w.IsArchived == false && w.Id == id).FirstOrDefaultAsync();
            
            return widget;
        }

        public async Task<(bool successful, string message)> CreateWidget(WidgetModel newWidget)
        {
            try
            {
                var result = await _context.Widget.AddAsync(newWidget);
                await _context.SaveChangesAsync();

                return (true , "Widget created successfully.");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
            
        }
    }
}
