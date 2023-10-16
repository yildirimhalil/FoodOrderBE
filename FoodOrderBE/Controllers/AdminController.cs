using FoodOrderBE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FoodOrderBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("addUpdateFood")]
        public Response addUpdateFood(Foods foods)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("FoodOrderCS").ToString());
            Response response = dal.addUpdateFood(foods, connection);
            return response;
        }

        [HttpGet]
        [Route("userList")]
        public Response userList()
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("FoodOrderCS").ToString());
            Response response = dal.userList(connection);
            return response;
        }
    }
}
