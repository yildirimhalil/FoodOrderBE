using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FoodOrderBE.Models
{
    public class DAL
    {
        private readonly IMongoDatabase _database;

        public DAL(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("FoodOrderCS");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("FoodOrder");
        }

        public Response Register(Users user)
        {
            var collection = _database.GetCollection<Users>("Users");
            Response response = new Response();

            try
            {
                collection.InsertOne(user);
                response.StatusCode = 200;
                response.StatusMessage = "Kullanıcı başarılı bir şekilde kaydedildi";
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Kullanıcı kaydı hatası: {ex.Message}";
            }

            return response;
        }

        public Response Login(Users user)
        {
            var collection = _database.GetCollection<Users>("Users");
            Response response = new Response();

            var filter = Builders<Users>.Filter.Eq("Email", user.Email) & Builders<Users>.Filter.Eq("Password", user.Password);
            var result = collection.Find(filter).FirstOrDefault();

            if (result != null)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Kullanıcı doğrulandı";
                response.User = result;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Kullanıcı doğrulanamadı";
                response.User = null;
            }

            return response;
        }

        public Response ViewUser(Users user)
        {
            var collection = _database.GetCollection<Users>("Users");
            Response response = new Response();

            var filter = Builders<Users>.Filter.Eq("_id", user.Id);
            var result = collection.Find(filter).FirstOrDefault();

            if (result != null)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Kullanıcı mevcut";
                response.User = result;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Kullanıcı mevcut değil";
                response.User = null;
            }

            return response;
        }

        public Response UpdateProfile(Users user)
        {
            var collection = _database.GetCollection<Users>("Users");
            Response response = new Response();

            var filter = Builders<Users>.Filter.Eq("_id", user.Id);
            var update = Builders<Users>.Update
                .Set("FirstName", user.FirstName)
                .Set("LastName", user.LastName)
                .Set("Password", user.Password)
                .Set("Email", user.Email);

            var result = collection.UpdateOne(filter, update);

            if (result.ModifiedCount > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Kullanıcı profili güncelleme başarılı";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Kullanıcı profili güncelleme başarılamadı!";
            }

            return response;
        }

        public Response AddToCart(Cart cart)
        {
            var collection = _database.GetCollection<Cart>("Carts");
            Response response = new Response();

            try
            {
                collection.InsertOne(cart);
                response.StatusCode = 200;
                response.StatusMessage = "Ürün sepete başarıyla eklendi";
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Ürün sepete eklenemedi: {ex.Message}";
            }

            return response;
        }

        public Response PlaceOrder(Users user)
        {
            var collection = _database.GetCollection<Orders>("Orders");
            Response response = new Response();

            try
            {
                var order = new Orders
                {
                    UserId = user.Id.ToString(),
                    OrderNo = GenerateOrderNumber(),
                    OrderTotal = CalculateOrderTotal(user.Id.ToString()),
                    OrderStatus = "Pending"
                };

                collection.InsertOne(order);
                response.StatusCode = 200;
                response.StatusMessage = "Sipariş başarıyla verildi";
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Sipariş verilemedi: {ex.Message}";
            }

            return response;
        }

        private decimal CalculateOrderTotal(string v)
        {
            throw new NotImplementedException();
        }

        public Response OrderList(Users user)
        {
            var orderCollection = _database.GetCollection<Orders>("Orders");
            var orderItemsCollection = _database.GetCollection<OrderItems>("OrderItems");

            Response response = new Response();
            List<Orders> orderList = new List<Orders>();

            var filter = Builders<Orders>.Filter.Eq("UserId", user.Id);
            var orders = orderCollection.Find(filter).ToList();

            foreach (var order in orders)
            {
                var orderItemsFilter = Builders<OrderItems>.Filter.Eq("OrderID", order.Id);
                var orderItemList = orderItemsCollection.Find(orderItemsFilter).ToList();

                orderList.Add(new Orders
                {
                    Id = order.Id,
                    OrderNo = order.OrderNo,
                    OrderTotal = order.OrderTotal,
                    OrderStatus = order.OrderStatus,
                    OrderItems = orderItemList
                });
            }

            if (orderList.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Sipariş detayları getirildi";
                response.ListOrders = orderList;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Sipariş detayları mevcut değil";
                response.ListOrders = null;
            }

            return response;
        }
        public Response UserList()
        {
            var collection = _database.GetCollection<Users>("Users");
            Response response = new Response();

            try
            {
                var userList = collection.Find(_ => true).ToList();
                response.StatusCode = 200;
                response.StatusMessage = "Kullanıcı listesi başarıyla getirildi";
                response.ListUsers = userList;
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Kullanıcı listesi getirilemedi: {ex.Message}";
                response.ListUsers = null;
            }

            return response;
        }

        public Response AddUpdateFood(Foods food)
        {
            var collection = _database.GetCollection<Foods>("Foods");
            Response response = new Response();

            try
            {
                // Aynı isme sahip yemek kontrolü
                var existingFood = collection.Find(f => f.Name == food.Name).FirstOrDefault();

                if (existingFood == null)
                {
                    // Eğer aynı isme sahip bir yemek yoksa, yeni bir yemek ekler
                    collection.InsertOne(food);
                    response.StatusCode = 200;
                    response.StatusMessage = "Yiyecek başarıyla eklendi";
                }
                else
                {
                    // Aynı isme sahip bir yemek varsa, güncelleme yapar
                    var filter = Builders<Foods>.Filter.Eq("_id", existingFood.Id);
                    var update = Builders<Foods>.Update
                        .Set(f => f.Name, food.Name)
                        .Set(f => f.Manufacturer, food.Manufacturer)
                        .Set(f => f.UnitPrice, food.UnitPrice)
                        .Set(f => f.Discount, food.Discount)
                        .Set(f => f.Quantity, food.Quantity)
                        .Set(f => f.ExpDate, food.ExpDate)
                        .Set(f => f.ImageUrl, food.ImageUrl)
                        .Set(f => f.Status, food.Status)
                        .Set(f => f.Type, food.Type);

                    var result = collection.UpdateOne(filter, update);

                    if (result.ModifiedCount > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Yiyecek güncelleme başarılı";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Yiyecek güncellenemedi";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Yiyecek ekleme/güncelleme başarısız: {ex.Message}";
            }

            return response;
        }

        public Response GetFoodDetails(string foodId)
        {
            var collection = _database.GetCollection<Foods>("Foods");
            Response response = new Response();

            try
            {
                var filter = Builders<Foods>.Filter.Eq("Id", ObjectId.Parse(foodId));
                var food = collection.Find(filter).FirstOrDefault();

                if (food != null)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Yiyecek detayları başarıyla getirildi";
                    response.Food = food;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Yiyecek detayları mevcut değil";
                    response.Food = null;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Yiyecek detayları getirilemedi: {ex.Message}";
                response.Food = null;
            }

            return response;
        }

        public Response DeleteFood(ObjectId foodId)
        {
            var collection = _database.GetCollection<Foods>("Foods");
            Response response = new Response();

            try
            {
                var filter = Builders<Foods>.Filter.Eq("_id", foodId);
                var result = collection.DeleteOne(filter);

                if (result.DeletedCount > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Yiyecek silme başarılı";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Yiyecek silinemedi";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = $"Bir hata oluştu: {ex.Message}";
            }

            return response;
        }


        public Response GetAllFoods()
        {
            var collection = _database.GetCollection<Foods>("Foods");
            Response response = new Response();

            try
            {
                var yemekler = collection.Find(_ => true).ToList();
                response.StatusCode = 200;
                response.StatusMessage = "Yemek listesi başarıyla alındı";
                response.ListFoods = yemekler;
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Yemek listesi alınamadı: {ex.Message}";
                response.ListFoods = null;
            }

            return response;
        }


        // Yardımcı metotlar
        private string GenerateOrderNumber()
        {

            return Guid.NewGuid().ToString();
        }

        private decimal CalculateOrderTotal(ObjectId userId)
        {

            return 100.00M;
        }
    }
}
