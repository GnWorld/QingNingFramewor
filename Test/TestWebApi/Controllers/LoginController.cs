using Microsoft.AspNetCore.Mvc;
using QingNing.MultiDbSqlSugar;
using SqlSugar;
using TestWebApi.Entity;
using TestWebApi.Exceptions;

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
            var b = _appRoleRep.InsertNow(new AppRole { RoleName = "admin", Code = "admin1" });
            var a = _appRoleRep.AsUpdateable(new AppRole { RoleId = 7, RoleName = "测试", Code = "test1" }).ExecuteCommand();
            var model = new MyClass { Name = "test" };
            ExceptionExtension.Throw(model, "测试报错");
            return Ok();
        }

    }
    public class MyClass
    {
        public string Name { get; set; }
    }
}
