using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WidgetService.Models;

namespace WidgetService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WidgetsController : ControllerBase
    {
        private readonly ILogger<WidgetsController> _logger;
        private readonly WidgetService.Services.WidgetService _widgetService;


        public WidgetsController(ILogger<WidgetsController> logger, Services.WidgetService widgetService)
        {
            _logger = logger;
            _widgetService = widgetService;
        }

        [HttpGet]
        public async Task<GetAllWidgetsResponseModel> Get()
        {
            var result = await _widgetService.GetAllNonArchivedWidgets();
            return result;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<WidgetModel> GetById(int id)
        {
            var result = await _widgetService.GetWidgetById(id);
            return result;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] WidgetModel newWidget)
        {
            newWidget.Id = 0;
            var result = await _widgetService.CreateWidget(newWidget);
            if (result.successful)
            {
                return Ok(new { Success = result.successful, Message = result.message });
            }
            return Problem( detail: result.message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
