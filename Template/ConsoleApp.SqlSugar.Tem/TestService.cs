using QingNing.MultiDbSqlSugar;
using SqlSugar;

namespace ConsoleApp.SqlSugar.Tem;
public class TestService
{
    private readonly SqlSugarRepository<AppRole> _testRep;

    public TestService(SqlSugarRepository<AppRole> testRep)
    {
        _testRep = testRep;
    }

    [UnitOfWork]
    public async Task Test()
    {
        var t = await _testRep.Context.SqlQueryable<AppRole>("select * from AppRole").ToListAsync();

        var a = await _testRep.InsertAsync(new AppRole { Id = 2, Code = "Admin", Name = "管理员" });
        var b = await _testRep.InsertAsync(new AppRole { Id = 2, Code = "Admin", Name = "管理员" });
        var c = await _testRep.InsertAsync(new AppRole { Id = 3, Code = "Admin", Name = "管理员" });
        var d = await _testRep.InsertAsync(new AppRole { Id = 3, Code = "Admin", Name = "管理员" });

    }

    [SugarTable(tableName: "AppRole")]
    public class AppRole
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

    }

}
