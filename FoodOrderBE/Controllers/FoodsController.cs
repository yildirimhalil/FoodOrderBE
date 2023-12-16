using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodOrderBE.Models;

namespace FoodOrderBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly DAL _dal;

        public FoodsController(DAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        [Route("addToCart")]
        public Response AddToCart(Cart cart)
        {
            Response response = _dal.AddToCart(cart);
            return response;
        }

        [HttpPost]
        [Route("placeOrder")]
        public Response PlaceOrder(Users user)
        {
            Response response = _dal.PlaceOrder(user);
            return response;
        }

        [HttpPost]
        [Route("orderList")]
        public Response OrderList(Users user)
        {
            Response response = _dal.OrderList(user);
            return response;
        }

        [HttpGet]
        [Route("getFoodDetails/{foodId}")]
        public Response GetFoodDetails(string foodId)
        {
            Response response = _dal.GetFoodDetails(foodId);
            return response;
        }
        [HttpGet]
        [Route("getAllFoods")]
        public Response GetAllFoods()
        {
            Response response = _dal.GetAllFoods();
            return response;
        }


    }
}
