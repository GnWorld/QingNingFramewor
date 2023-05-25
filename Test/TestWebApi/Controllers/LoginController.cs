using Microsoft.AspNetCore.Mvc;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {


        public LoginController()
        {

        }

        [HttpGet]
        public IActionResult Test()
        {

            //var a = _roleRep.Select.ToList();
            return Ok();
        }

    }
}
