using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderBE.Models
{
    public class Response
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        [BsonIgnoreIfNull]
        public List<Users> ListUsers { get; set; }

        [BsonIgnoreIfNull]
        public Users User { get; set; }

        [BsonIgnoreIfNull]
        public List<Foods> ListFoods { get; set; }

        [BsonIgnoreIfNull]
        public Foods Food { get; set; }

        [BsonIgnoreIfNull]
        public List<Cart> ListCart { get; set; }

        [BsonIgnoreIfNull]
        public List<Orders> ListOrders { get; set; }

        [BsonIgnoreIfNull]
        public Orders Order { get; set; }

        [BsonIgnoreIfNull]
        public List<OrderItems> ListItems { get; set; }

        [BsonIgnoreIfNull]
        public OrderItems OrderItem { get; set; }
    }
}