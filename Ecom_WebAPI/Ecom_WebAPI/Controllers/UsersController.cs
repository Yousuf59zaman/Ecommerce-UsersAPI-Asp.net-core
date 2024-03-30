using Ecom_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

   
        DAL _dal = new DAL();
        
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            bool result = _dal.RegisterUser(user,connectionString);
            if (result)
            {
                return Ok(new Response<User> { Success = true, Message = "Registration successful", Data = user });
            }

            return BadRequest(new Response<User> { Success = false, Message = "Registration failed" });
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            User user = _dal.AuthenticateUser(loginUser.Email, loginUser.Password, connectionString);

            if (user != null)
            {
                // In a real application, generate a token or session ID here
                return Ok(new Response<User> { Success = true, Message = "Login successful", Data = user });
            }

            return Unauthorized(new Response<User> { Success = false, Message = "Login failed" });
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            User user = _dal.GetUserById(userId, connectionString);
            if (user != null)
            {
                return Ok(new Response<User> { Success = true, Message = "User found", Data = user });
            }

            return NotFound(new Response<User> { Success = false, Message = "User not found" });
        }

        
        [HttpPut]
        [Route("update")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            bool result = _dal.UpdateUser(user, connectionString);
            if (result)
            {
                return Ok(new Response<User> { Success = true, Message = "User updated successfully", Data = user });
            }

            return BadRequest(new Response<User> { Success = false, Message = "User update failed" });
        }



    }
}




