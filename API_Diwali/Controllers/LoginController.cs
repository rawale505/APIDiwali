using API_Diwali.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Diwali.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        public ILoginServices _loginServices;
        public LoginController(ILoginServices loginController)
        {
            _loginServices = loginController;
        }

        [HttpGet]
        [Route("UserLogin")]
        public IActionResult AdminLogin([FromHeader] string username, [FromHeader] string password)
         {
            var result = _loginServices.UserLoginServices(username, password);
            if (result == null)
            {
                return Unauthorized("Wrong Credentials");
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost]
        [Route("User_Registration")]
        public IActionResult UserRegistration(string Username, string Password, string emailId, string Role)
        {
            var result = _loginServices.UserRegistrationServices(Username, Password, emailId, Role);
            if (result == null)
            {
                return BadRequest("Registration Failed.... Try With Other Credentials");
            }
            else
            {
                return Ok("User Registered Successfully.");
            }
        }
    }
}
