using Microsoft.AspNetCore.Mvc;
using QingNing.MultiDbSqlSugar;
using SqlSugar;
using TestWebApi.Entity;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SqlSugarRepository<AppRole> _appRoleRep;

        public LoginController(SqlSugarRepository<AppRole> appRoleRep)
        {
            _appRoleRep = appRoleRep;
        }

        [HttpGet]
        [UnitOfWork]
        public IActionResult Test()
        {
            var b = _appRoleRep.Insert(new AppRole { RoleName = "admin", Code = "admin" });
            var a = _appRoleRep.AsUpdateable(new AppRole { RoleId = 7, RoleName = "测试", Code = "test1" }).ExecuteCommand();

            return Ok();
        }

    }
}
