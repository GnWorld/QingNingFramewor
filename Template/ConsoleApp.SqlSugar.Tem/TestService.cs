using Newtonsoft.Json;
using QingNing.MultiDbSqlSugar;
using QingNing.MultiDbSqlSugar.Attributes;
using QingNing.MultiDbSqlSugar.UOW;
using SqlSugar;
using StackExchange.Profiling.Internal;

namespace ConsoleApp.SqlSugar.Tem;

public interface ITestService
{
    Task Test();
}

public class TestService : ITestService
{
    private readonly SqlSugarRepository<AppRole> _roleRep;
    private readonly IUnitOfWorkManage _unitOfWorkManage;

    public TestService(SqlSugarRepository<AppRole> testRep, IUnitOfWorkManage unitOfWorkManage)
    {
        _roleRep = testRep;
        _unitOfWorkManage = unitOfWorkManage;
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
            //await _roleRep.Context.Ado.ExecuteCommandAsync("delete from AppRole");
 
        //var t = await _testRep.Context.SqlQueryable<AppRole>("select * from AppRole").ToListAsync();

        //var a = await _testRep.InsertAsync(new AppRole { Id = 2, Code = "Admin", Name = "管理员" });
        //var b = await _testRep.InsertAsync(new AppRole { Id = 2, Code = "Admin", Name = "管理员" });
        //var c = await _testRep.InsertAsync(new AppRole { Id = 3, Code = "Admin", Name = "管理员" });
        //var d = await _testRep.InsertAsync(new AppRole { Id = 3, Code = "Admin", Name = "管理员" });

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
    }
}
