using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderBE.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int ID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public int UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public decimal UnitPrice { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public decimal Discount { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public int Quantity { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public decimal TotalPrice { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public int FoodID { get; set; }
    }
}
