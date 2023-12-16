using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodOrderBE.Models
{
    public class Orders
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public string OrderNo { get; set; }

        public decimal OrderTotal { get; set; }

        public string OrderStatus { get; set; }

        public List<OrderItems> OrderItems { get; set; }
    }
}
