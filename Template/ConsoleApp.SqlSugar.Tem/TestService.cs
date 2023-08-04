using QingNing.MultiDbSqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.SqlSugar.Tem;
public class TestService
{
    private readonly SqlSugarRepository<TestModel> _context;
    private readonly SimpleClient<TestModel> _IRep;

    public TestService(SqlSugarRepository<TestModel> context, SimpleClient<TestModel> iRep)
    {
        _context = context;
        _IRep = iRep;
    }

    public async Task Test()
    {
        var t = await _context.Context.SqlQueryable<TestModel>("select * from AppRole").ToListAsync();
        var b = await _IRep.Context.SqlQueryable<TestModel>("select * from AppRole").ToListAsync();
    }

    public class TestModel
    {

        public long role_id { get; set; }

        public string role_name { get; set; }

        public string Code { get; set; }

    }

}
