using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodOrderBE.Models;

namespace FoodOrderBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DAL _dal;

        public UsersController(DAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        [Route("registration")]
        public Response Register(Users user)
        {
            Response response = _dal.Register(user);
            return response;
        }

        [HttpPost]
        [Route("login")]
        public Response Login(Users user)
        {
            Response response = _dal.Login(user);
            return response;
        }

        [HttpPost]
        [Route("viewUser")]
        public Response ViewUser(Users user)
        {
            Response response = _dal.ViewUser(user);
            return response;
        }

        [HttpPost]
        [Route("updateProfile")]
        public Response UpdateProfile(Users user)
        {
            Response response = _dal.UpdateProfile(user);
            return response;
        }
    }
}
