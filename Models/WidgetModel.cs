using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WidgetService.Models
{
    public class WidgetModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Sku { get; set; } = "";

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";
        public decimal UnitPrice { get; set; } = 0;
        public int QuantityAvailable { get; set; } = 0;
        public bool IsArchived { get; set; } = false;
    }
}
