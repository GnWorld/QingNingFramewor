using MultiSugarTestApi.Controllers;
using Newtonsoft.Json;
using QingNing.Extensions;
using QingNing.MultiDbSqlSugar;
using QingNing.MultiDbSqlSugar.Attributes;
using QingNing.MultiDbSqlSugar.UOW;
using SqlSugar;

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


    //[UnitOfWork]
    public async Task Test()
    {
        try
        {
            var list = new List<GtPath>() { new GtPath { P1 = "1", P2 = "1" }, new GtPath { P1 = "2", P2 = "2" } };
            AppRole? role1 = new AppRole()
            {
                RoleId = 1,
                RoleName = "role1",
                Code = "role1",
                Path = list

            };
            await _roleRep.InsertAsync(role1);

            List<AppRole>? a = await _roleRep.AsQueryable().ToListAsync();
        }
        catch (Exception ex)
        {

            throw;
        }

        //var role2 = new AppRole()
        //{
        //    RoleName = "role2",
        //    RoleId = 2,
        //    Code = "role2",
        //};


        //_roleRep.Insert(role2);



        Console.WriteLine("测试方法");

    }


}

[SugarTable]
public class AppRole
{
    /// <summary>
    /// 角色ID
    /// </summary>
    [JsonProperty, SugarColumn(ColumnName = "role_id", IsPrimaryKey = true, IsIdentity = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    [JsonProperty, SugarColumn(ColumnName = "role_name", ColumnDataType = "varchar(60)")]
    public string RoleName { get; set; } = string.Empty;

    [SugarColumn(IsNullable = false)]
    public string Code { get; set; }

    [SugarColumn(ColumnDataType ="jsonb",IsJson = true)]
    public List<GtPath> Path { get; set; }
}


public class GtPath
{
    public string P1 { get; set; }

    public string P2 { get; set; }
}
