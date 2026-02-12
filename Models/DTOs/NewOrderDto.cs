namespace WidgetService.Models.DTOs
{
    public class NewOrderDto
    {
        public List<NewOrderItem> Items { get; set; }
    }

    public class NewOrderItem
    {
        public int WidgetId { get; set; }
        public int Quantity { get; set; }
    }
}
