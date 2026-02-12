using static System.Runtime.InteropServices.JavaScript.JSType;
using Boolean = System.Boolean;

namespace WidgetService.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }
    public class GetAllWidgetsResponseModel
    {
        public List<WidgetModel> Data { get; set; }
        public int Count { get; set; }

    }

    public class OrderSuccessResponseModel : ResponseModel
    {
        public string OrderReference { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class BadRequestResponseModel : ErrorModel
    {
        public BadRequestResponseModel()
        {
            Code = "INSUFFICIENT_STOCK";
            Error = "Insufficient Stock";
            Details = [];
        }
    }
    public class UnauthorizedResponseModel : ErrorModel
    {
        public UnauthorizedResponseModel()
        {
            Code = "UNAUTHORIZED";
            Error = "Invalid or missing API key";
        }
    }
    public class NotFoundResponseModel : ErrorModel
    {
        public NotFoundResponseModel()
        {
            Code = "WIDGET_NOT_FOUND";
            Error = "Widget not found";
        }
    }
    public class ErrorModel : ResponseModel
    {
        public string Error { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<Details>? Details { get; set; }
    }

    public class Details
    {

        public int WidgetId { get; set; }
        public string WidgetName { get; set; }
        public int RequestedQuantity { get; set; }
        public int AvailableQuantity { get; set; }

    }
}
