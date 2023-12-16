using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodOrderBE.Models
{
    public class OrderItems
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string FoodId { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
