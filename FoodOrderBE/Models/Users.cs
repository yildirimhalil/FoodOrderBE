using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodOrderBE.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Password { get; set; } = "";

        public string Email { get; set; } = "";

        public decimal Fund { get; set; }

        public string Type { get; set; }

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
