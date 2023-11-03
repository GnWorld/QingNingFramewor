using MultiSugarTestApi.Controllers;
using QingNing.MultiDbSqlSugar;
using QingNing.MultiDbSqlSugar.Attributes;
using QingNing.MultiDbSqlSugar.UOW;

namespace MultiSugarTestApi;


public interface ITestService
{

    Task Test();
    Task Test2();
}


public class TestService : ITestService
{

    private readonly SqlSugarRepository<AppRole> _roleRep;
    private readonly IUnitOfWorkManage _unitOfWorkManage;


    public TestService(SqlSugarRepository<AppRole> roleRep, IUnitOfWorkManage unitOfWorkManage)
    {
        _roleRep = roleRep;
        _unitOfWorkManage = unitOfWorkManage;
    }

    [UnitOfWork(Propagation = Propagation.Nested)]
    public async Task Test2()
    {
        var role1 = new AppRole()
        {
            RoleId = 1,
            RoleName = "role",
            Code = null

        };

        _roleRep.Insert(role1);
        await Test();

    }


    [UnitOfWork]
    public async Task Test()
    {
        var role1 = new AppRole()
        {
            RoleId = 1,
            RoleName = "role1",
            Code = "role1"

        };
        _roleRep.Insert(role1);

        var role2 = new AppRole()
        {
            RoleName = "role2",
            RoleId = 2,
            Code = "role2",
        };


        _roleRep.Insert(role2);


        await _roleRep.AsQueryable().ToListAsync();

        Console.WriteLine("测试方法");

    }


}
