using System.Text.Json.Serialization;

namespace ECommerce.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryExpected { get; set; }
        [JsonIgnore]
        public bool ContainsGift { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
