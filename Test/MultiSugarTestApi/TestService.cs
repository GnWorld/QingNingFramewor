using MultiSugarTestApi.Controllers;
using QingNing.MultiDbSqlSugar;
using QingNing.MultiDbSqlSugar.Attributes;

namespace MultiSugarTestApi;

public class TestService : BaseService<AppRole>
{

    private readonly SqlSugarRepository<AppRole> _roleRep;

    public TestService(SqlSugarRepository<AppRole> roleRep)
    {
        _roleRep = roleRep;
    }

    [UnitOfWork]
    public async Task Test()
    {
        var role = new AppRole()
        {
            RoleId = 1,
            RoleName = "test",
            Code = "Test",
        };

        await _roleRep.InsertAsync(role);

        var role2 = new AppRole()
        {
            RoleId = 1,
            RoleName = null,
            Code = "Test",
        };

        await _roleRep.InsertAsync(role2);

    }


}
