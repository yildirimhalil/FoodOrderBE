using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FoodOrderBE.Models; 
using MongoDB.Bson;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly DAL _dal;

    public AdminController(DAL dal)
    {
        _dal = dal;
    }

    [HttpPost]
    [Route("addUpdateFood")]
    public Response AddUpdateFood(Foods food)
    {
        Response response = _dal.AddUpdateFood(food);
        return response;
    }

    [HttpGet]
    [Route("userList")]
    public Response UserList()
    {
        Response response = _dal.UserList();
        return response;
    }

    [HttpDelete]
    [Route("deleteFood/{foodId}")]
    public Response DeleteFood(string foodId)
    {
        Response response = new Response();

        try
        {
            
            if (ObjectId.TryParse(foodId, out ObjectId objectId))
            {
                response = _dal.DeleteFood(objectId);
            }
            else
            {
                // Hatalı ObjectId
                response.StatusCode = 400; // BadRequest durumunu belirt
                response.StatusMessage = "Invalid ObjectId";
            }
        }
        catch (Exception ex)
        {
            response.StatusCode = 500; // Internal Server Error durumunu belirt
            response.StatusMessage = $"An error occurred: {ex.Message}";
        }

        return response;
    }
}
